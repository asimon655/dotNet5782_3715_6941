using Mapsui.Utilities;
using PL.Map;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for MapTab.xaml
    /// </summary>
    public partial class MapTab : Page
    {
        public void Reset()
        {

            #region Remove all Layers 
            foreach (var layer in MyMapControl.Map.Layers.Skip(1))
            {
                MyMapControl.Map.Layers.Remove(layer);

            }
            #endregion
            #region Map Initialize 

            IEnumerable<int> idUser = from user in dat.GetCustomers() select user.Id;
            IEnumerable<int> idStation = from stat in dat.GetStations() select stat.Id;
            IEnumerable<int> ids = from drn in dat.GetDrones() select drn.Id;
            IEnumerable<string> Models = from drn in dat.GetDrones() select drn.Model;
            IEnumerable<BO.Location>[] ALLPOINTSMOVED = MapHELP.SetPoints(dat);
            MapHELP.DrawPointsOnMap(MyMapControl, ALLPOINTSMOVED[2], idUser, 0.1, "\\PL\\Images\\user.png", true);
            MapHELP.DrawPointsOnMap(MyMapControl, ALLPOINTSMOVED[1], idStation, 0.25, "\\PL\\Images\\BASESTATION.png");
            MapHELP.DrawPointsOnMap(MyMapControl, ALLPOINTSMOVED[0], ids, 0.45, null, false, Models);
            MyMapControl.Refresh(); 
            #endregion
        }
        internal async Task ResetLoct()
        {
            IEnumerable<BO.DroneList> NewLocts = await Task.Run(() => dat.GetDrones());
             MapHELP.ResetLoct(MyMapControl, NewLocts);



        }
        private void ReturnHome(int Dur = 500)
        {
            var bbox = new Mapsui.Geometries.BoundingBox(new BO.Location(34.732433, 31.987003).ToPlPoint(), new BO.Location(34.988032, 32.166204).ToPlPoint());
            MyMapControl.Navigator.NavigateTo(bbox, ScaleMethod.Fit);
            MyMapControl.Navigator.ZoomTo(40, Dur);
            MyMapControl.Refresh();
        }

        private readonly BlApi.Ibl dat;
        public MapTab(BlApi.Ibl dat)
        {

            InitializeComponent();
            MyMapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            MyMapControl.Map.BackColor = Mapsui.Styles.Color.FromArgb(255, 171, 210, 223);
            ReturnHome(0);

            this.dat = dat;
            Reset();
        }
        private void ChangeOpacity(int index)
        {
            index++; 
            if (MyMapControl.Map.Layers.Skip(index ).First().Opacity == 0)
            {
                MyMapControl.Map.Layers.Skip(index ).First().Opacity = 1;
            }
            else
            {
                MyMapControl.Map.Layers.Skip(index ).First().Opacity = 0;
            }

            MyMapControl.Refresh();


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReturnHome();

        }
        private void OpcL1(object sender, RoutedEventArgs e)
        {
            ChangeOpacity(2);
        }
        private void OpcL2(object sender, RoutedEventArgs e)
        {
            ChangeOpacity(1);
        }
        private void OpcL3(object sender, RoutedEventArgs e)
        {
            ChangeOpacity(0);
        }

    }
}

using BruTile.Predefined;
using BruTile.Web;
using Mapsui.Layers;
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
            //	https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png working normal 
            // 	https://cartodb-basemaps-{s}.global.ssl.fastly.net/dark_all/{z}/{x}/{y}.png working  dark
            //  https://cartodb-basemaps-{s}.global.ssl.fastly.net/light_all/{z}/{x}/{y}.png working bright
            //  https://tile.thunderforest.com/atlas/{z}/{x}/{y}.png?apikey=a4e9d225293242168650a97e010ee288wotking regular better then def 
            //  https://tile.thunderforest.com/pioneer/{z}/{x}/{y}.png?apikey=a4e9d225293242168650a97e010ee288 old westy style 
            //  
            var tileSource = new HttpTileSource(new GlobalSphericalMercator(), "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", new[] { "a", "b", "c" }, name: "OpenStreetMap", userAgent: "OpenStreetMap in Mapsui");
            var tileLayer = new TileLayer(tileSource)
            {
                Name = "OSM",
            };
            MyMapControl.Map.Layers.Add(tileLayer);
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

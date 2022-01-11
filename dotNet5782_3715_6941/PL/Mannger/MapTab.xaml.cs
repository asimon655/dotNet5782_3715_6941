using Mapsui.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MapTab.xaml
    /// </summary>
    public partial class MapTab : Page
    {
        public void Reset() {

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
           MapHELP.DrawPointsOnMap(MyMapControl,ALLPOINTSMOVED[0], ids, 0.45, null, false, Models);
            MapHELP.DrawPointsOnMap(MyMapControl, ALLPOINTSMOVED[1], idStation, 0.25, "\\PL\\Images\\BASESTATION.png");
            MapHELP.DrawPointsOnMap(MyMapControl, ALLPOINTSMOVED[2], idUser, 0.1, "\\PL\\Images\\user.png", true);
            #endregion
        }
        public void ResetLoct()
        {
            IEnumerable<BO.DroneList> NewLocts = dat.GetDrones();
            MapHELP.ResetLoct(MyMapControl, NewLocts);
            MyMapControl.Refresh();


        }
        BlApi.Ibl dat;
        public MapTab(BlApi.Ibl dat)
        {
     
            InitializeComponent();
            MyMapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            MyMapControl.Map.BackColor = Mapsui.Styles.Color.FromArgb(255, 171, 210, 223);
            this.dat = dat;
            Reset();
        }
    }
}

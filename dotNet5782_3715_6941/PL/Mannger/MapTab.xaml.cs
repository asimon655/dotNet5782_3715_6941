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
using System.Windows.Controls.Primitives;

namespace PL
{
    /// <summary>
    /// Interaction logic for MapTab.xaml
    /// </summary>
    public partial class MapTab : Page
    {

        private readonly BlApi.Ibl dat;

        #region ResetFunctions

        public void Reset()
        {
            MyMapControl.Refresh();
        }

        internal async Task ResetLoct()
        {
            IEnumerable<BO.DroneList> NewLocts = await Task.Run(() => dat.GetDrones());
            await Task.Run(() => MapHELP.ResetLoct(MyMapControl, NewLocts));
        }

        #endregion
        public  void RemoveAllPoints() 
        {
            var baseLayer = MyMapControl.Map.Layers.First();
            MyMapControl.Map.Layers.Clear();
            MyMapControl.Map.Layers.Insert(0, baseLayer);




        }



        private void ReturnHome(int Dur = 500)
        {
            var bbox = new Mapsui.Geometries.BoundingBox(new BO.Location(34.732433, 31.987003).ToPlPoint(), new BO.Location(34.988032, 32.166204).ToPlPoint());
            MyMapControl.Navigator.NavigateTo(bbox, ScaleMethod.Fit);
            MyMapControl.Navigator.ZoomTo(40, Dur);
            MyMapControl.Refresh();
        }

        public MapTab(BlApi.Ibl dat)
        {

            InitializeComponent();

            ExestionsionMap.getMapDBObject().ContinueWith(x => Dispatcher.Invoke( ()=> Maps.ItemsSource = x.Result.maps ) );


            Task task = MyMapControl.Map.TaskLoadTheme(
                (object obj)=> Dispatcher.Invoke(() => {
                    MyMapControl.Refresh(); 
                } )
            ); // we load it in it is own time , no pressure 
         

            ReturnHome(0);

            this.dat = dat;
            drawAllPoints();
            Reset();
        }

        public void drawAllPoints() {
            IEnumerable<BO.DroneList> drones = dat.GetDrones();
            IEnumerable<BO.CustomerList> costumers = dat.GetCustomers();
            IEnumerable<BO.StationList> station = dat.GetStations();
           
        
            drawCostumers(costumers);
            drawStats(station);
            drawDrones(drones);



        }



        public void drawStats(IEnumerable<BO.StationList> station) {
            MapHELP.DrawPointsOnMap(MyMapControl, station.Select(stt => dat.GetStation(stt.Id).LoctConstant), station.Select(stt => stt.Id), 0.25, "\\PL\\Images\\BASESTATION.png");

        }

        public void drawstat(BO.Station stat)  { 
        
        }

        public void drawDrones(IEnumerable<BO.DroneList> drones)
        {
            MapHELP.DrawPointsOnMap(MyMapControl, drones.Select(drn => drn.Loct), drones.Select(drn => drn.Id), 0.45, null, false, drones.Select(drn => drn.Model));

        }

        public void drawDrone(BO.Drone drobe) { 
        
        }
        public void drawCostumers(IEnumerable<BO.CustomerList> costumers) {
            MapHELP.DrawPointsOnMap(MyMapControl, costumers.Select(cst => dat.GetCostumer(cst.Id).Loct), costumers.Select(cst => cst.Id), 0.1, "\\PL\\Images\\user.png", true);

        }
        public void drawCostumer(BO.CustomerList costumer) { 
            
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

        private void OpcL(object sender, RoutedEventArgs e)
        {
            ChangeOpacity(int.Parse((sender as ToggleButton).Tag.ToString()));
        }

        private void Maps_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            Task task = MyMapControl.Map.TaskLoadTheme(  
                  (object obj) => Dispatcher.Invoke(() => {
                      MyMapControl.Refresh();
                  }), ((sender as ListView).SelectedItem as MapsDBObject.MapObject).name

              );
            task.ContinueWith(t => MessageBox.Show("Map Loaded"));
            Settings.IsChecked = false;
        }
    }
}

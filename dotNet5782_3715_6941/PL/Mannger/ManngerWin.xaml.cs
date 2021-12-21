using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPFSpark;
using Mapsui.Utilities;
using Mapsui.Layers;
using HarfBuzzSharp;
using Mapsui.Styles;
using Mapsui.Providers;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManngerWin : SparkWindow
    {

        List<CheckBoxStatus> predStat;

        #region Predicts 
        public Predicate<T> combine<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) && ConvertorNullable<T>(b)(x));
        Predicate<T> OrGate<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) || ConvertorNullable<T>(b)(x));
        Predicate<T> ConvertorNullable<T>(Predicate<T>? a) => (a is null ? new Predicate<T>(x => true) : a);
        public IEnumerable<BO.WeightCategories> Weight;
        public IEnumerable<BO.DroneStatuses> Stat;
        public IEnumerable<BO.WeightCategories> WeightDefault = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
        public IEnumerable<BO.DroneStatuses> StatDefault = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
        #endregion

        #region Fields
        public Stat status;
        BlApi.Ibl dat;
        #endregion

        #region Ctor 
        public ManngerWin(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            #region Map Initialize 
            MyMapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            MyMapControl.Map.BackColor = Mapsui.Styles.Color.FromArgb(255, 171, 210, 223);
            IEnumerable<int> idUser = from user in dat.GetCustomers() select user.Id;
            IEnumerable<int> idStation = from stat in dat.GetStations() select stat.Id;
            IEnumerable<int> ids = from drn in dat.GetDrones() select drn.Id;
            IEnumerable<string> Models = from drn in dat.GetDrones() select drn.Model;
            IEnumerable<BO.Location>[] ALLPOINTSMOVED = SetPoints();
            DrawPointsOnMap(ALLPOINTSMOVED[0], ids,0.4,null, Models);
            DrawPointsOnMap(ALLPOINTSMOVED[1], idStation,0.3, "\\PL\\Images\\BASESTATION.png");
            DrawPointsOnMap(ALLPOINTSMOVED[2], idUser,0.1, "\\PL\\Images\\user.png");
            #endregion

            #region Plots Initialize 
            BO.DronesModelsStats dronesStats = dat.GetDronesModelsStats();
            createModelsBar(WpfPlot2, dronesStats.pos, dronesStats.names, dronesStats.vals);
            CreateDountPie<BO.WeightCategories>(WpfPlot1, dat.GetDronesWeightsStats());
            CreateDountPie<BO.DroneStatuses>(WpfPlot3, dat.GetDronesStatusesStats());
            CreateDountPie<BO.ParcelStatus>(WpfPlotPack1, dat.GetParcelsStatusesStats());
            CreateDountPie<BO.WeightCategories>(WpfPlotPack2, dat.GetParcelsWeightsStats());
            CreateDountPie<BO.Priorities>(WpfPlotPack3, dat.GetParcelsPrioretiesStats());
            #endregion

            #region Predicts Initialize 
            ListOf.ItemsSource = dat.GetDrones();
            Weight = WeightDefault;
            Stat = StatDefault;
            predStat = new List<CheckBoxStatus>();
            foreach (BO.DroneStatuses enm in StatDefault)
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
            #endregion

            #region ListView Initialize 
            ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
            ListOfPackges.ItemsSource = dat.GetParcels();
            #endregion

            #region ListView Grouping 
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfPackges.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ParcelStatus");
            view.GroupDescriptions.Add(groupDescription);
            #endregion

        }
        #endregion

        #region Buttons Functions
        // reset button action


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Weight = WeightDefault;
            Stat = StatDefault;
            //StatusSelectorWeigthStat.SelectedIndex = -1;
            //StatusSelectorDrnStat.SelectedIndex = -1;
            foreach (CheckBoxStatus item in predStat)
            {
                item.Checked = true;
            }
            ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
        }


        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((sender as ComboBox).SelectedItem is null))
            {
                Weight = new List<BO.WeightCategories>() { (BO.WeightCategories)(sender as ComboBox).SelectedItem };
            }
            ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
        }



        private void ListOf_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new Window2(dat.GetDrone(((sender as ListView).SelectedItem as BO.DroneList).Id)).Show();

        }


        static bool IsSorted<T>(IEnumerable<T> enumerable) where T : IComparable<T>
        {
            T prev = default(T);
            bool prevSet = false;
            foreach (var item in enumerable)
            {
                if (prevSet && (prev == null || prev.CompareTo(item) > 0))
                    return false;
                prev = item;
                prevSet = true;
            }
            return true;
        }


        private void Battery_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("Id", "Id");
            dict.Add("Model", "Model");
            dict.Add("Battery", "Battery");
            dict.Add("Location", "Loct");
            dict.Add("MaxWeight", "Weight");
            dict.Add("DroneStatus", "DroneStat");
            dict.Add("Binded Parcel Id", "ParcelId");
            object IdLst = (object)
            MessageBox.Show((e.OriginalSource as GridViewColumnHeader).Column.Header.ToString());
            ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneList>().OrderBy(x => typeof(BO.DroneList).GetProperty(dict[(e.OriginalSource as GridViewColumnHeader).Column.Header.ToString()]).GetValue(x, null));
            ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneList>().Reverse();
        }
        #endregion
    }
}

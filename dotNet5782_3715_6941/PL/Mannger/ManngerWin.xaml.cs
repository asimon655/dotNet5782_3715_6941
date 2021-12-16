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
        public Predicate<T> combine<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) && ConvertorNullable<T>(b)(x));
        Predicate<T> OrGate<T>(Predicate<T>? a, Predicate<T>? b) => new Predicate<T>(x => ConvertorNullable<T>(a)(x) || ConvertorNullable<T>(b)(x));
        Predicate<T> ConvertorNullable<T>(Predicate<T>? a) => (a is null ? new Predicate<T>(x => true) : a);
        public IEnumerable<BO.WeightCategories> Weight;
        public IEnumerable<BO.DroneStatuses> Stat;
        public IEnumerable<BO.WeightCategories> WeightDefault = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
        public IEnumerable<BO.DroneStatuses> StatDefault = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
        public Stat status;
        BlApi.Ibl dat;
        public ManngerWin(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            Random rng = new Random();
            MyMapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            MyMapControl.Map.BackColor = Mapsui.Styles.Color.FromArgb(255,171, 210, 223);
            var ly = new Mapsui.Layers.WritableLayer();
            Mapsui.Geometries.Point pt;
            Mapsui.Providers.Feature feature;
            Mapsui.Styles.VectorStyle x;
            foreach (var drn in dat.GetDrones())
            {
                double px = (6371 * 1000) * Math.Sin(drn.Loct.Lattitude) * Math.Cos(drn.Loct.Longitude);
                double py = (6371 * 1000) * Math.Cos(drn.Loct.Lattitude);
                pt = new Mapsui.Geometries.Point(px, py);
                feature = new Mapsui.Providers.Feature { Geometry = pt };
                x = new Mapsui.Styles.VectorStyle() { Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.FromArgb(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256))) };
                feature.Styles.Add(x);
                ly.Add((IFeature)feature);
            }


            MyMapControl.Map.Layers.Add(ly);
            MyMapControl.Refresh();

            IEnumerable<BO.DroneList> Dronelst = this.dat.GetDrones();
            IEnumerable<BO.ParcelList> Parcellst = dat.GetParcels();
            string[] names = GetNaemsDrones(Dronelst);
            double[] vals = GetValsDrone(Dronelst);
            double[] pos = GetPosDrones(vals.Length);
            createModelsBar(WpfPlot2, pos, names, vals);
            CreateDountPie<BO.WeightCategories>(WpfPlot1, GetValsDroneWeight(Dronelst));
            CreateDountPie<BO.DroneStatuses>(WpfPlot3, GetValsDroneStat(Dronelst));
            CreateDountPie<BO.ParcelStat>(WpfPlotPack1, GetValsPackgesParcelStat(Parcellst));
            CreateDountPie<BO.WeightCategories>(WpfPlotPack2, GetValsPackgesWeight(Parcellst));
            CreateDountPie<BO.Priorities>(WpfPlotPack3, GetValsPackgesPrioreties(Parcellst));
            WpfPlot2.Plot.Style(ScottPlot.Style.Blue3);
            WpfPlot3.Plot.Style(ScottPlot.Style.Blue3);
            WpfPlot1.Plot.Style(ScottPlot.Style.Blue3);
            ListOf.ItemsSource = dat.GetDrones();
            Weight = WeightDefault;
            Stat = StatDefault;
            predStat = new List<CheckBoxStatus>();
            foreach (BO.DroneStatuses enm in StatDefault)
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
            ListOf.ItemsSource = dat.GetDronesFiltered(Stat, Weight);
            ListOfPackges.ItemsSource = dat.GetParcels();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfPackges.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ParcelStatus");
            view.GroupDescriptions.Add(groupDescription);
        }

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
    }
}

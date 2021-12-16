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
        bool cf = false;



        BlApi.Ibl dat;
        internal int LengthStart { get; set; }
        internal void createModelsBar(ScottPlot.WpfPlot Bar, double[] pos, string[] names, double[] vals)
        {
            Bar.Plot.Clear();
            Bar.Plot.AddBar(vals, pos, ColorTranslator.FromHtml("#6600cc"));
            Bar.Plot.XTicks(pos, names);
            Bar.Plot.SetAxisLimits(yMin: 0);
            Bar.Refresh();


        }
        internal void CreateSingleGauge<T>(ScottPlot.WpfPlot Gauge, double[] values) {

            Gauge.Plot.Clear();
            Gauge.Plot.Palette = ScottPlot.Drawing.Palette.Nord;

            var gauges = Gauge.Plot.AddRadialGauge(values);
            gauges.Clockwise = false;

            Gauge.Plot.AxisAuto(0);

            Gauge.Render();

        }
        internal void CreateDountPie<T>(ScottPlot.WpfPlot Pie, double[] values)
        {
            Pie.Plot.Clear();
            string[] labels = Enum.GetNames(typeof(T));


            // Language colors from https://github.com/ozh/github-colors
            System.Drawing.Color[] sliceColors =
            {
                ColorTranslator.FromHtml("#DBCDC6"),
                ColorTranslator.FromHtml("#DD99BB"),
                ColorTranslator.FromHtml("#7B506F"),
                ColorTranslator.FromHtml("#1F1A38"),
                ColorTranslator.FromHtml("#C7EFCF"),
};

            // Show labels using different transparencies
            System.Drawing.Color[] labelColors =
                new System.Drawing.Color[] {
     System.Drawing.Color.FromArgb(255,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(100,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(250,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(150,  System.Drawing.Color.White),
     System.Drawing.Color.FromArgb(200,  System.Drawing.Color.White),
            };

            var pie = Pie.Plot.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowPercentages = true;

            pie.SliceFillColors = sliceColors;
            pie.SliceLabelColors = labelColors;
            Pie.Render();


        }
        internal string[] GetNaemsDrones(IEnumerable<BO.DroneList> Dronelst)
        {
            IEnumerable<string> Models = from Drony in Dronelst select Drony.Model;
            Models = Models.Distinct();
            return Models.ToArray();

        }
        internal double[] GetPosDrones(int size)
        {
            double[] positions = new double[size];
            for (int i = 0; i < size; i++)
            {

                positions[i] = i;
            }

            return positions;

        }
        internal double[] GetValsDrone(IEnumerable<BO.DroneList> Dronelst)
        {
            string[] Models = GetNaemsDrones(Dronelst);
            double[] values2 = new double[Models.Count()];
            for (int i = 0; i < values2.Length; i++)
                values2[i] = Dronelst.Count(x => x.Model == Models.Skip(i).First());
            return values2;


        }
        internal double[] GetValsDroneStat(IEnumerable<BO.DroneList> Dronelst)
        {
            BO.DroneStatuses[] Stats = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
            IEnumerable<double> filtered = from stat in Stats
                                           where Dronelst.Count(x => x.DroneStat == stat) > 0
                                           select Convert.ToDouble(Dronelst.Count(x => x.DroneStat == stat));
            return filtered.ToArray();


        }
        internal double[] GetValsDroneWeight(IEnumerable<BO.DroneList> Dronelst)
        {
            BO.WeightCategories[] Weights = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
            IEnumerable<double> filtered = from weight in Weights
                                           where Dronelst.Count(x => x.Weight == weight) > 0
                                           select Convert.ToDouble(Dronelst.Count(x => x.Weight == weight));
            return filtered.ToArray();


        }
        internal double[] GetValsPackgesPrioreties(IEnumerable<BO.ParcelList> Parcellst)
        {
            BO.Priorities[] Stats = (BO.Priorities[])Enum.GetValues(typeof(BO.Priorities));
            IEnumerable<double> filtered = from stat in Stats
                                           where Parcellst.Count(x => x.Priorety == stat) > 0
                                           select Convert.ToDouble(Parcellst.Count(x => x.Priorety == stat));
            return filtered.ToArray();

        }

        internal double[] GetValsPackgesParcelStat(IEnumerable<BO.ParcelList> Parcellst)
        {
            BO.ParcelStat[] Stats = (BO.ParcelStat[])Enum.GetValues(typeof(BO.ParcelStat));
            IEnumerable<double> filtered = from stat in Stats
                                           where Parcellst.Count(x => x.ParcelStatus == stat) > 0
                                           select Convert.ToDouble(Parcellst.Count(x => x.ParcelStatus == stat));
            return filtered.ToArray();


        }
        internal double[] GetValsPackgesWeight(IEnumerable<BO.ParcelList> Parcellst)
        {
            BO.WeightCategories[] Stats = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
            IEnumerable<double> filtered = from stat in Stats
                                           where Parcellst.Count(x => x.Weight == stat) > 0
                                           select Convert.ToDouble(Parcellst.Count(x => x.Weight == stat));
            return filtered.ToArray();


        }


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
            LengthStart = vals.Length;
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
            //new Window2(a).Show();

        }



        private void chkCountry_Checked(object sender, RoutedEventArgs e)
        {
           // if (!(StatusSelectorDrnStat.ItemsSource is null))
            //{
              //  Stat = predStat.Where(x => x.Checked).Select(x => x.statusof);
            //}
            //ListOf.ItemsSource = a.DronesPrintFiltered(Stat, Weight);


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

        private void Closing_Thread(object sender, CancelEventArgs e)
        {
            cf = false;
        }

        private void ListOf1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

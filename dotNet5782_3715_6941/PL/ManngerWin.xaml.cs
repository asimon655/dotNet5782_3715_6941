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
        internal int LengthStart { get; set;  }
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
        internal void CreateDountPie<T>(ScottPlot.WpfPlot Pie ,double[] values  )
        {
            Pie.Plot.Clear();
            string[] labels = Enum.GetNames(typeof(T));


            // Language colors from https://github.com/ozh/github-colors
            System.Drawing.Color[] sliceColors =
            {
                ColorTranslator.FromHtml("#0000ff"),
                ColorTranslator.FromHtml("#6600cc"),
                ColorTranslator.FromHtml("#ff6699"),
                ColorTranslator.FromHtml("#B845FC"),
                ColorTranslator.FromHtml("#4F5D95"),
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
        internal string[] GetNaemsDrones(IEnumerable<BO.DroneToList> Dronelst)
        {
            IEnumerable<string> Models = from Drony in Dronelst select Drony.Model;
            Models = Models.Distinct();
            return Models.ToArray();

        }
        internal double [] GetPosDrones(int size)
        {
            double[] positions = new double[size];
            for (int i = 0; i < size; i++)
            {
                
                positions[i] = i ;
            }

            return positions;

        }
        internal double []  GetValsDrone(IEnumerable<BO.DroneToList> Dronelst)
        {
            string[] Models = GetNaemsDrones(Dronelst);
            double[] values2 = new double[Models.Count()];
            for (int i = 0; i < values2.Length; i++)
                values2[i] = Dronelst.Count(x => x.Model == Models.Skip(i).First());
            return values2; 


        }
        internal double[] GetValsDroneWeight(IEnumerable<BO.DroneToList> Dronelst )
        {
            BO.WeightCategories []  Weights = (BO.WeightCategories [] )Enum.GetValues(typeof(BO.WeightCategories));
            IEnumerable<double> filtered = from weight in Weights
                                        where Dronelst.Count(x => x.Weight == weight) > 0
                                        select Convert.ToDouble(Dronelst.Count(x => x.Weight == weight));
            return filtered.ToArray();


        }
        internal double[] GetValsDroneStat(IEnumerable<BO.DroneToList> Dronelst)
        {
            BO.DroneStatuses[] Stats = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
            IEnumerable<double> filtered = from stat in Stats
                                           where Dronelst.Count(x => x.DroneStat == stat) > 0
                                           select Convert.ToDouble(Dronelst.Count(x => x.DroneStat == stat));
            return filtered.ToArray();


        }
        public ManngerWin(BlApi.Ibl dat)
        {
            this.dat = dat;
            InitializeComponent();
            IEnumerable<BO.DroneToList> Dronelst = this.dat.DronesPrint();
            string[] names = GetNaemsDrones(Dronelst);
            double[] vals = GetValsDrone(Dronelst);
            LengthStart = vals.Length; 
            double[] pos = GetPosDrones(vals.Length );
            createModelsBar(WpfPlot2, pos, names, vals);
            CreateDountPie<BO.WeightCategories>(WpfPlot1, GetValsDroneWeight(Dronelst));
            //CreateSingleGauge<BO.DroneStatuses>(WpfPlot3, GetValsDroneStat(Dronelst));
            CreateDountPie<BO.DroneStatuses>(WpfPlot3, GetValsDroneStat(Dronelst));
            WpfPlot2.Plot.Style(ScottPlot.Style.Blue3);
            WpfPlot3.Plot.Style(ScottPlot.Style.Blue3);
            WpfPlot1.Plot.Style(ScottPlot.Style.Blue3);
            ListOf.ItemsSource = dat.DronesPrint();
            Weight = WeightDefault;
            Stat = StatDefault;

            predStat = new List<CheckBoxStatus>();
            foreach (BO.DroneStatuses enm in StatDefault)
                predStat.Add(new CheckBoxStatus() { Checked = true, statusof = enm });
           // StatusSelectorDrnStat.ItemsSource = predStat;
            //StatusSelectorWeigthStat.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));


            ListOf.ItemsSource = dat.DronesPrintFiltered(Stat, Weight);


                Item1Ani.Message = new WPFSpark.StatusMessage("");
                Item1Ani.FontSize = 160;
                Item1Ani.FontWeight = FontWeights.Bold;
        
                Item1Ani.FadeOutDirection = StatusDirection.Left;
            Item1Ani.FadeOutDistance = 0;
            Item1Ani.FadeOutDuration = new Duration(new TimeSpan(0, 0, 0, 0, 30));

            Thread thread = new Thread(UpdateText);

            thread.Start();



        }



        private void UpdateText()
        {
            while (true)
            {
                string text = "hello";
               
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                    for (int i = 0; i < text.Length; i++)
                    {

                        Item1Ani.SetStatus(text.Substring(0,i+1), true);
                          
                        }
                    }));


                Thread.Sleep(TimeSpan.FromSeconds(5));

                text = "hola";
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    for (int i = 0; i < text.Length; i++)
                    {

                        Item1Ani.SetStatus(text.Substring(0, i + 1), true);
         
                    }

                }));
                Thread.Sleep(TimeSpan.FromSeconds(5));
                text = "hallo";
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    for (int i = 0; i < text.Length; i++)
                    {


                        Item1Ani.SetStatus(text.Substring(0, i + 1), true);

                    }

                }));

                Thread.Sleep(TimeSpan.FromSeconds(5));
                text = "שלום";
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    for (int i = 0; i < text.Length; i++)
                    {


                        Item1Ani.SetStatus(text.Substring(0, i + 1), true);

                    }

                }));

                Thread.Sleep(TimeSpan.FromSeconds(5));

            }

            

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
            ListOf.ItemsSource = dat.DronesPrintFiltered(Stat, Weight);
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((sender as ComboBox).SelectedItem is null))
            {
                Weight = new List<BO.WeightCategories>() { (BO.WeightCategories)(sender as ComboBox).SelectedItem };
            }
            ListOf.ItemsSource = dat.DronesPrintFiltered(Stat, Weight);
        }

        private void ListOf_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new Window2(dat.PullDataDrone(((sender as ListView).SelectedItem as BO.DroneToList).Id)).Show();
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
            dict.Add("Battery", "BatteryStat");
            dict.Add("Location", "Current");
            dict.Add("MaxWeight", "Weight");
            dict.Add("DroneStatus", "DroneStat");
            dict.Add("Binded Parcel Id", "ParcelIdTransfer");
            object IdLst = (object)
            MessageBox.Show((e.OriginalSource as GridViewColumnHeader).Column.Header.ToString());

            ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneToList>().OrderBy(x => typeof(BO.DroneToList).GetProperty(dict[(e.OriginalSource as GridViewColumnHeader).Column.Header.ToString()]).GetValue(x, null));
            ListOf.ItemsSource = ListOf.ItemsSource.Cast<BO.DroneToList>().Reverse();
        }





    }
}

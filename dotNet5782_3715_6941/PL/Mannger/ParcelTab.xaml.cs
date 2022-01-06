using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ParcelTab.xaml
    /// </summary>
    public partial class ParcelTab : Page
    {
        #region Fields 
        BlApi.Ibl dat;
        #endregion
        public ParcelTab(BlApi.Ibl dat )
        {
            InitializeComponent();
            this.dat = dat;

            #region ListView Grouping 
            ListOf.ItemsSource = dat.GetParcels();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOf.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ParcelStatus");
            view.GroupDescriptions.Add(groupDescription);
            #endregion

            #region Plots Initialize 
            CreateDountPie<BO.ParcelStatus>(WpfPlotPack1, dat.GetParcelsStatusesStats());
            CreateDountPie<BO.WeightCategories>(WpfPlotPack2, dat.GetParcelsWeightsStats());
            CreateDountPie<BO.Priorities>(WpfPlotPack3, dat.GetParcelsPrioretiesStats());
            #endregion

        }


        #region ScottPlot
        internal void createModelsBar(ScottPlot.WpfPlot Bar, double[] pos, string[] names, double[] vals)
        {
            Bar.Plot.Clear();
            Bar.Plot.AddBar(vals, pos, ColorTranslator.FromHtml("#6600cc"));
            Bar.Plot.XTicks(pos, names);
            Bar.Plot.SetAxisLimits(yMin: 0);
            Bar.Refresh();


        }
        internal void CreateSingleGauge<T>(ScottPlot.WpfPlot Gauge, double[] values)
        {

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


        #endregion

        private void ListOfPackges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelShow(dat, dat.GetParcel(((sender as ListView).SelectedItem as BO.ParcelList).Id)).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window add = new ParcelShow(dat); 
            add.Closed += (sender, e) =>
            {
                ListOf.ItemsSource = dat.GetParcels();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOf.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("ParcelStatus");
                view.GroupDescriptions.Add(groupDescription);
            };
            add.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
      
                int id = (int)(sender as Button).Tag;
                dat.DeleteParcel(id);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
            ListOf.ItemsSource = dat.GetParcels();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListOf.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ParcelStatus");
            view.GroupDescriptions.Add(groupDescription);
        }
    }
}

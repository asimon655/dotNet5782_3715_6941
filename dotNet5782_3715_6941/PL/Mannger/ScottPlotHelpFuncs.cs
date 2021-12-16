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





    }
}
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
    public partial class ManngerWin : SparkWindow
    {
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

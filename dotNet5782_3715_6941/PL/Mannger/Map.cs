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
            private const double Radius = 6378137;
            private const double E = 0.0000000848191908426;
            private const double D2R = Math.PI / 180;
            private const double PiDiv4 = Math.PI / 4;
            Mapsui.Geometries.Point FromLonLat(double lon, double lat)
            {
                var lonRadians = D2R * lon;
                var latRadians = D2R * lat;

                var x = Radius * lonRadians;
                //y=a×ln[tan(π/4+φ/2)×((1-e×sinφ)/(1+e×sinφ))^(e/2)]
                var y = Radius * Math.Log(Math.Tan(PiDiv4 + latRadians * 0.5) / Math.Pow(Math.Tan(PiDiv4 + Math.Asin(E * Math.Sin(latRadians)) / 2), E));

                return new Mapsui.Geometries.Point(x, y);
            }


    }
}
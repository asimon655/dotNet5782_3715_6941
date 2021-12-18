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
using System.IO;

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
        private  int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                var memoryStream = new MemoryStream();
                fs.CopyTo(memoryStream);
                var bitmapId = BitmapRegistry.Instance.Register(memoryStream);
                return bitmapId;
            }

        }
        private  SymbolStyle CreateSymbolStyle(string embeddedResourcePath, double scale)
        {
            var bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
            return new SymbolStyle { BitmapId = bitmapId, SymbolType = SymbolType.Ellipse, SymbolScale = scale, SymbolOffset = new Offset(0.0, 0.0, true) };
        }
        void DrawPointsOnMap(IEnumerable<BO.Location> points , IEnumerable<int> ids  ,IEnumerable<string>Models)
        {
            Random rng = new Random();
            var ly = new Mapsui.Layers.WritableLayer();
            Mapsui.Geometries.Point pt;
            Mapsui.Providers.Feature feature;
            Mapsui.Styles.LabelStyle x;
            Mapsui.Styles.VectorStyle x2;
            Mapsui.Styles.Color BGColor;
            
            ly.Style = null;

            for (int i=0; i<points.Count(); i++)
            {
                pt = FromLonLat(points.Skip(i).First().Longitude, points.Skip(i).First().Lattitude);
                feature = new Mapsui.Providers.Feature { Geometry = pt };
                BGColor = Mapsui.Styles.Color.FromArgb(
                            rng.Next(120, 256),
                            rng.Next(120, 256),
                            rng.Next(120, 256),
                            0);
             x = new Mapsui.Styles.LabelStyle() {
                Text = ids.Skip(i).First().ToString(),
                Font = new Mapsui.Styles.Font { FontFamily = "Courier New", Bold = true, Italic = true, },
                BackColor = new Mapsui.Styles.Brush(BGColor),
                ForeColor = BGColor,
                Halo = new Mapsui.Styles.Pen(Mapsui.Styles.Color.Black, 2),
                HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                MaxWidth = 10,
                WordWrap = LabelStyle.LineBreakMode.TailTruncation
            };
                x2 = new Mapsui.Styles.VectorStyle
                {
                    Fill = new Mapsui.Styles.Brush(BGColor),
                };
                
                feature.Styles.Add(x2);
                if (!File.Exists(Window2.TMP + @"image" + Models.Skip(i).First().Replace(" ", "_") + ".png"))
                    Window2.SaveFirstImage(Models.Skip(i).First());
                feature.Styles.Add(CreateSymbolStyle(Window2.TMP + @"image" + Models.Skip(i).First().Replace(" ", "_") + ".png", 0.4));
                feature.Styles.Add(x);
                ly.Add((IFeature)feature);
            }


            MyMapControl.Map.Layers.Add(ly);
            MyMapControl.Refresh();
        }



    }
}
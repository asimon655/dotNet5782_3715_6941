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
using System.Drawing.Imaging;

namespace PL
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public static class MapHELP
    {




        #region Internal Functions
        #region Constans 
        private const double Radius = 6378137;
        private const double E = 0.0000000848191908426;
        private const double D2R = Math.PI / 180;
        private const double PiDiv4 = Math.PI / 4;
        #endregion
        internal static Mapsui.Geometries.Point FromLonLat(double lon, double lat)
        {
            var lonRadians = D2R * lon;
            var latRadians = D2R * lat;

            var x = Radius * lonRadians;
            //y=a×ln[tan(π/4+φ/2)×((1-e×sinφ)/(1+e×sinφ))^(e/2)]
            var y = Radius * Math.Log(Math.Tan(PiDiv4 + latRadians * 0.5) / Math.Pow(Math.Tan(PiDiv4 + Math.Asin(E * Math.Sin(latRadians)) / 2), E));

            return new Mapsui.Geometries.Point(x, y);
        }
        private static int? GetBitmapIdForEmbeddedResource(string imagePath)
        {
            int val;
            if (!Cache.TryGetValue(imagePath, out val))
            {
                try
                {
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open))
                    {
                        var memoryStream = new MemoryStream();
                        fs.CopyTo(memoryStream);
                        var bitmapId = BitmapRegistry.Instance.Register(memoryStream);
                        Cache.Add(imagePath, bitmapId);
                        return bitmapId;
                    }

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error");
                    return null;

                }

            }

            return val;


        }
        private static IEnumerable<BO.Location>[] MovePoints(BlApi.Ibl dat, IEnumerable<BO.Location> pointsToDraw, int[] StartIndexes) {
            List<BO.Location> pointsonce = new List<BO.Location>();
            {
                int i = 0;
                foreach (var loct in pointsToDraw)
                {
                    if (pointsToDraw.Skip(i).Count(x => x.Lattitude == loct.Lattitude && loct.Longitude == x.Longitude) > 1)
                        pointsonce.Add(new BO.Location(
                            loct.Longitude + 0.0001 * pointsToDraw.Skip(i).Count(x => x.Lattitude == loct.Lattitude && loct.Longitude == x.Longitude)
                            , loct.Lattitude + 0.0001 * pointsToDraw.Skip(i).Count(x => x.Lattitude == loct.Lattitude && loct.Longitude == x.Longitude)
                            ));
                    else
                        pointsonce.Add(new BO.Location(loct.Longitude, loct.Lattitude));
                    i++;

                }
            }
            IEnumerable<BO.Location>[] ReturnVal = new IEnumerable<BO.Location>[StartIndexes.Length];
            for (int i = 0; i < StartIndexes.Length; i++)
            {
                if (i == (StartIndexes.Length - 1))
                    ReturnVal[i] = pointsonce.GetRange(StartIndexes[i], pointsToDraw.Count() - StartIndexes[i]);
                else ReturnVal[i] = pointsonce.GetRange(StartIndexes[i], StartIndexes[i + 1] - StartIndexes[i]);
            }
            return ReturnVal;


        }
        private static SymbolStyle CreateSymbolStyle(string embeddedResourcePath, double scale)
        {
            int? bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
            if (!(bitmapId is null))
                return new SymbolStyle { BitmapId = (int)bitmapId, SymbolType = SymbolType.Ellipse, SymbolScale = scale, SymbolOffset = new Offset(0.0, 0.0, true) };
            return null;
        }
        #endregion

        #region Fields
        private static List<Mapsui.Geometries.Point> pointsList = new List<Mapsui.Geometries.Point>();
        #region Cache
        private static Dictionary<String, int> Cache = new Dictionary<String, int>();
        #endregion
        #endregion

        #region MapTabFuncs
        internal static IEnumerable<BO.Location>[] SetPoints(BlApi.Ibl dat)
        {
            IEnumerable<int> idUser = from user in dat.GetCustomers() select user.Id;
            IEnumerable<int> idStation = from stat in dat.GetStations() select stat.Id;
            IEnumerable<BO.Location> DronePoints = from drn in dat.GetDrones() select drn.Loct;
            IEnumerable<BO.Location> StationsPoints = from statID in idStation select dat.GetStation(statID).LoctConstant;
            IEnumerable<BO.Location> UserPoints = from userID in idUser select dat.GetCostumer(userID).Loct;
            List<BO.Location> ALLPOINTS = new List<BO.Location>();
            foreach (var x in DronePoints)
                ALLPOINTS.Add(x);
            foreach (var x in StationsPoints)
                ALLPOINTS.Add(x);
            foreach (var x in UserPoints)
                ALLPOINTS.Add(x);
            int[] StartsIndexes = new int[3] { 0, DronePoints.Count(), DronePoints.Count() + StationsPoints.Count() };
            return MovePoints(dat, ALLPOINTS, StartsIndexes);
        }
        internal static async void DrawPointsOnMap(Mapsui.UI.Wpf.MapControl MyMapControl, IEnumerable<BO.Location> points, IEnumerable<int> ids, double scale, string? path, bool FILL = false, IEnumerable<string>? Names = null)
        {

            Random rng = new Random();
            var ly = new Mapsui.Layers.WritableLayer();
            Mapsui.Geometries.Point pt;
            Mapsui.Providers.Feature feature;
            Mapsui.Styles.LabelStyle x;
            Mapsui.Styles.VectorStyle x2;
            Mapsui.Styles.Color BGColor;
            ly.Style = null;
            for (int i = 0; i < points.Count(); i++)
            {
                pt = FromLonLat(points.Skip(i).First().Longitude, points.Skip(i).First().Lattitude);
                feature = new Mapsui.Providers.Feature { Geometry = pt };
                BGColor = Mapsui.Styles.Color.FromArgb(
                            rng.Next(120, 256),
                            rng.Next(120, 256),
                            rng.Next(120, 256),
                            0);
                x = new Mapsui.Styles.LabelStyle()
                {
                    Text = ids.Skip(i).First().ToString(),
                    Font = new Mapsui.Styles.Font { FontFamily = "Courier New", Bold = true, Italic = true, },
                    ForeColor = BGColor,
                    Halo = new Mapsui.Styles.Pen(Mapsui.Styles.Color.Black, 1),
                    HorizontalAlignment = LabelStyle.HorizontalAlignmentEnum.Left,
                    MaxWidth = 10,
                    WordWrap = LabelStyle.LineBreakMode.TailTruncation 
                    
                };
               
                if (FILL)
                {
                    x2 = new Mapsui.Styles.VectorStyle
                    {
                        Fill = new Mapsui.Styles.Brush(BGColor),
                    };
                    feature.Styles.Add(x2);
                }
                if (path is null)
                {
                    pointsList.Add(pt);
                    if (!File.Exists(PhotoAsync.makePath(Names.Skip(i).First())))
                        await PhotoAsync.SaveFirstImageAsync(Names.Skip(i).First());
                    
                     feature.Styles.Add(CreateSymbolStyle(PhotoAsync.makePath(Names.Skip(i).First()), scale));
                 
                      
                    
                }
                else
                {
                    feature.Styles.Add(CreateSymbolStyle(Directory.GetCurrentDirectory() + path, scale));

                }
                feature.Styles.Add(x);
                ly.Add((IFeature)feature);
            }


            MyMapControl.Map.Layers.Add(ly);
            MyMapControl.Refresh();
        }
        internal static void ResetLoct(Mapsui.UI.Wpf.MapControl MyMapControl, IEnumerable<BO.DroneList> NewLoct)
        {
            for (int i = 0; i < NewLoct.Count(); i++)
            {
                Mapsui.Geometries.Point tmp = FromLonLat(NewLoct.Skip(i).First().Loct.Longitude, NewLoct.Skip(i).First().Loct.Lattitude);
                pointsList.Skip(i).First().X = tmp.X;
                pointsList.Skip(i).First().Y = tmp.Y;
            }
        
        
        
        }



        #endregion

    }
}
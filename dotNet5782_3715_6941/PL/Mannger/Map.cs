using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
        private static IEnumerable<BO.Location>[] MovePoints(BlApi.Ibl dat, IEnumerable<BO.Location> pointsToDraw, int[] StartIndexes)
        {
            List<BO.Location> pointsonce = new List<BO.Location>();
            {
                int i = 0;
                foreach (var loct in pointsToDraw)
                {
                    if (pointsToDraw.Skip(i).Count(x => x.Lattitude == loct.Lattitude && loct.Longitude == x.Longitude) > 1)
                    {
                        pointsonce.Add(new BO.Location(
                            loct.Longitude + 0.0001 * pointsToDraw.Skip(i).Count(x => x.Lattitude == loct.Lattitude && loct.Longitude == x.Longitude)
                            , loct.Lattitude + 0.0001 * pointsToDraw.Skip(i).Count(x => x.Lattitude == loct.Lattitude && loct.Longitude == x.Longitude)
                            ));
                    }
                    else
                    {
                        pointsonce.Add(new BO.Location(loct.Longitude, loct.Lattitude));
                    }

                    i++;

                }
            }
            IEnumerable<BO.Location>[] ReturnVal = new IEnumerable<BO.Location>[StartIndexes.Length];
            for (int i = 0; i < StartIndexes.Length; i++)
            {
                if (i == (StartIndexes.Length - 1))
                {
                    ReturnVal[i] = pointsonce.GetRange(StartIndexes[i], pointsToDraw.Count() - StartIndexes[i]);
                }
                else
                {
                    ReturnVal[i] = pointsonce.GetRange(StartIndexes[i], StartIndexes[i + 1] - StartIndexes[i]);
                }
            }
            return ReturnVal;


        }
        private static SymbolStyle CreateSymbolStyle(string embeddedResourcePath, double ? scale)
        {
            if (scale is null)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(@"c:\ggs\ggs Access\images\members\1.jpg");
                return new SymbolStyle { BitmapId = (int)bitmapId, SymbolType = SymbolType.Ellipse, SymbolScale = File., SymbolOffset = new Offset(0.0, 0.0, true) };

            }
            else
            {
                int? bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
                if (!(bitmapId is null))
                {
                    return new SymbolStyle { BitmapId = (int)bitmapId, SymbolType = SymbolType.Ellipse, SymbolScale = (double)scale, SymbolOffset = new Offset(0.0, 0.0, true) };
                }
                return null;
            }
            
        }
        #endregion

        #region Fields
        #region Cache
        private static readonly Dictionary<string, int> Cache = new Dictionary<string, int>();
        private static readonly Dictionary<int, Mapsui.Geometries.Point> pointsMannger = new Dictionary<int, Mapsui.Geometries.Point>();
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
            {
                ALLPOINTS.Add(x);
            }

            foreach (var x in StationsPoints)
            {
                ALLPOINTS.Add(x);
            }

            foreach (var x in UserPoints)
            {
                ALLPOINTS.Add(x);
            }

            int[] StartsIndexes = new int[3] { 0, DronePoints.Count(), DronePoints.Count() + StationsPoints.Count() };
            return MovePoints(dat, ALLPOINTS, StartsIndexes);
        }
        private static async Task<IFeature> CreateFeature(double scale, double Longitude, double Lattitude, int Id, bool FILL = false, string? path = null, string? Name = null)
        {
            Random rng = new Random();
            Mapsui.Geometries.Point pt;
            Mapsui.Providers.Feature feature;
            Mapsui.Styles.LabelStyle x;
            Mapsui.Styles.VectorStyle x2;
            Mapsui.Styles.Color BGColor;
            pt = FromLonLat(Longitude, Lattitude);

            feature = new Mapsui.Providers.Feature { Geometry = pt };
            BGColor = Mapsui.Styles.Color.FromArgb(
                        rng.Next(120, 256),
                        rng.Next(120, 256),
                        rng.Next(120, 256),
                        0);
            x = new Mapsui.Styles.LabelStyle()
            {
                Text = Id.ToString(),
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
            if (path is null && !(Name is null))
            {
                pointsMannger.TryAdd(Id, pt);

                if (!File.Exists(PhotoAsync.makePath(Name)))
                {
                    PhotoAsync.SaveFirstImageAsync(Name).ContinueWith(x => { feature.Styles.Add(CreateSymbolStyle(PhotoAsync.makePath(Name), null)); });
                }
                else
                {
                    feature.Styles.Add(CreateSymbolStyle(PhotoAsync.makePath(Name), scale));
                }



            }
            else
            {
                feature.Styles.Add(CreateSymbolStyle(Directory.GetCurrentDirectory() + path, scale));

            }
            feature.Styles.Add(x);
            return feature;






        }
        internal static async Task DrawPointsOnMap(Mapsui.UI.Wpf.MapControl MyMapControl, IEnumerable<BO.Location> points, IEnumerable<int> ids, double scale, string? path, bool FILL = false, IEnumerable<string>? Names = null)
        {


            var ly = new Mapsui.Layers.WritableLayer
            {
                Style = null
            };
            for (int i = 0; i < points.Count(); i++)
            {

                ly.Add(await CreateFeature(scale
                    , points.Skip(i).First().Longitude,
                    points.Skip(i).First().Lattitude,
                    ids.Skip(i).First(),
                    FILL, path,
                    (Names is null ? null : Names.Skip(i).First())




                    ));

            }


            MyMapControl.Map.Layers.Add(ly);
            MyMapControl.Refresh();
        }
        internal static async Task ResetLoct(Mapsui.UI.Wpf.MapControl MyMapControl, IEnumerable<BO.DroneList> NewLoct)
        {
            for (int i = 0; i < NewLoct.Count(); i++)
            {
                Mapsui.Geometries.Point tmp = FromLonLat(NewLoct.Skip(i).First().Loct.Longitude, NewLoct.Skip(i).First().Loct.Lattitude);
                Mapsui.Geometries.Point point;
                if (pointsMannger.TryGetValue(NewLoct.Skip(i).First().Id, out point))
                {
                    point.X = tmp.X;
                    point.Y = tmp.Y;
                }
                else
                {
                    IFeature feature = await CreateFeature(0.45
                    , NewLoct.Skip(i).First().Loct.Longitude,
                    NewLoct.Skip(i).First().Loct.Lattitude,
                    NewLoct.Skip(i).First().Id,
                    false, null,
                    NewLoct.Skip(i).First().Model);
                    (MyMapControl.Map.Layers.Skip(3).First() as WritableLayer).Add(feature);


                }





            }



        }



        #endregion

    }
}
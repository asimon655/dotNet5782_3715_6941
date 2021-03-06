using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;
using PL.Map;
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
       
        private static int? GetBitmapIdForEmbeddedResource(string imagePath)//creates BitMap on the RAM for the Photo 
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
        private static IEnumerable<BO.Location>[] MovePoints(BlApi.Ibl dat, IEnumerable<BO.Location> pointsToDraw, int[] StartIndexes)//If there any collisions it moves the points 
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
        private static SymbolStyle CreateSymbolStyle(string embeddedResourcePath, double ? scale)//creates Image(SYMBOL) to add to the Feature List of the point 
        {
       
            
                int? bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
                if (!(bitmapId is null))
                {
                    return new SymbolStyle { BitmapId = (int)bitmapId, SymbolType = SymbolType.Ellipse, SymbolScale = (double)scale, SymbolOffset = new Offset(0.0, 0.0, true) };
                }
                return null;
         
            
        }
        #endregion

        #region Fields
        #region Cache
        private static readonly Dictionary<string, int> Cache = new Dictionary<string, int>(); //Cache to prevent bitmap of the same Photo Twice 
        private static readonly Dictionary<int, Mapsui.Geometries.Point> pointsMannger = new Dictionary<int, Mapsui.Geometries.Point>(); //pointsMannger Cache  to update The drones Location 
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
        public  static IFeature CreateFeature(double scale,BO.Location loct , int Id, bool FILL = false, string? path = null, string? Name = null)
        {
            Random rng = new Random();
            Mapsui.Geometries.Point pt;
            Mapsui.Providers.Feature feature;
            Mapsui.Styles.LabelStyle x;
            Mapsui.Styles.VectorStyle x2;
            Mapsui.Styles.Color BGColor;
            pt = loct.ToPlPoint(); 

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
            if (path is null && !(Name is null)) //for Scraping 
            {
                pointsMannger.TryAdd(Id, pt);

                if (!File.Exists(PhotoAsync.makePath(Name)))
                {
                     PhotoAsync.SaveFirstImageAsync(Name).ContinueWith(x => { if(x.Result)feature.Styles.Add(CreateSymbolStyle(PhotoAsync.makePath(Name), scale)); }); 
                }
                else
                    feature.Styles.Add(CreateSymbolStyle(PhotoAsync.makePath(Name), scale));



            }
            else //const Image 
            {
                feature.Styles.Add(CreateSymbolStyle(Directory.GetCurrentDirectory() + path, scale));

            }
            feature.Styles.Add(x);
            return feature;






        }
        internal static void DrawPointsOnMap(Mapsui.UI.Wpf.MapControl MyMapControl, IEnumerable<BO.Location> points, IEnumerable<int> ids, double scale, string? path, bool FILL = false, IEnumerable<string>? Names = null)
        {


            var ly = new Mapsui.Layers.WritableLayer
            {
                Style = null
            };
            for (int i = 0; i < points.Count(); i++)
            {

                ly.Add(CreateFeature(scale
                    , points.Skip(i).First(),
                    ids.Skip(i).First(),
                    FILL, path,
                    (Names is null ? null : Names.Skip(i).First())
                    ));

            }


            MyMapControl.Map.Layers.Add(ly);
            MyMapControl.Refresh();
        }
        internal static  void  ResetLoct(Mapsui.UI.Wpf.MapControl MyMapControl, IEnumerable<BO.DroneList> NewLoct)
        {
            for (int i = 0; i < NewLoct.Count(); i++)
            {
                Mapsui.Geometries.Point tmp =NewLoct.Skip(i).First().Loct.ToPlPoint();
                Mapsui.Geometries.Point point;
                if (pointsMannger.TryGetValue(NewLoct.Skip(i).First().Id, out point))
                {
                    point.X = tmp.X;
                    point.Y = tmp.Y;
                }
                else
                {
                    IFeature feature = CreateFeature(0.45
                    , NewLoct.Skip(i).First().Loct,
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
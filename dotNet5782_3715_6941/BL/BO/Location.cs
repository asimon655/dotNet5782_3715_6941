using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BO
{

    public struct Location : IComparable
    {


        public Location(double longitude, double lattitude)
        {
            Longitude = longitude;
            Lattitude = lattitude;
        }

        public Location(Itinero.LocalGeo.Coordinate coordinate)
        {
            Longitude = coordinate.Longitude;
            Lattitude = coordinate.Latitude;
        }

        public double Longitude { set; get; }
        public double Lattitude { set; get; }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj.GetType() != typeof(Location))
            {
                return 1;
            }

            Location loct = (Location)obj;

            return (Longitude + Lattitude).CompareTo(loct.Lattitude + loct.Longitude);
        }

        public class Tile
        { // PSD  
            public int x;
            public int y;
            public int z;
        }

        public Tile ToTileLoct( int zoomSize)
        {
            Func<double, double> toRad = (double deg) => ((deg % 360) / 180 * Math.PI);
            Tile res = new Tile();
            int n = (int)Math.Pow(2, zoomSize);
            res.x = (int)(n * ((this.Longitude + 180) / 360));
            res.y = (int)(n * (1 - (Math.Log(Math.Tan(toRad(this.Lattitude)) + 1 / Math.Cos(toRad(this.Lattitude))) / Math.PI)) / 2);
            res.z = zoomSize;
            return res;

        }

        public async Task<byte[]> getLoactionPhoto(string mapLink, int zoomLevel) { // visual view of the image . - the server downloads the image and sends it to the client . 
            var tileLoct = this.ToTileLoct(zoomLevel);
            var tokens = new Dictionary<string, string> {

                { "{x}", tileLoct.x.ToString() },
                { "{y}", tileLoct.y.ToString() },
                { "{z}", tileLoct.z.ToString() },
                { "{s}", "a" } 
            };
            var r = new Regex(string.Join("|", tokens.Keys.Select(Regex.Escape)));
            var me = new MatchEvaluator(m => tokens[m.Value]);
            string parsedMapLink =  r.Replace(mapLink, me);
            WebClient client = new WebClient();
            byte[] data = await client.DownloadDataTaskAsync(parsedMapLink);
            return data;
        }


        public string DecimalToSexagesimal() /// calacs it with the well known algorithem that we found olnline ( beacuse u didnt gave that to us ) 
        {
            string result = "";
            // Longitude
            bool direction = (0 > Longitude);
            Longitude = Math.Abs(Longitude);
            result += Math.Floor(Longitude).ToString() + '\xb0';
            Longitude -= Math.Floor(Longitude);
            Longitude *= 60;
            result += Math.Floor(Longitude).ToString() + '`';
            Longitude -= Math.Floor(Longitude);
            Longitude *= 60;
            result += Math.Round(Longitude, 4).ToString() + "``";
            result += (direction ? 'N' : 'S');

            result += ' ';

            // Lattitude
            direction = (0 > Lattitude);
            Lattitude = Math.Abs(Lattitude);
            result += Math.Floor(Lattitude).ToString() + '\xb0';
            Lattitude -= Math.Floor(Lattitude);
            Lattitude *= 60;
            result += Math.Floor(Lattitude).ToString() + '`';
            Lattitude -= Math.Floor(Lattitude);
            Lattitude *= 60;
            result += Math.Round(Lattitude, 4).ToString() + "``";
            result += (direction ? 'E' : 'W');

            return result;
        }

        public override string ToString()
        {
            return DecimalToSexagesimal();
        }
 
    }
}

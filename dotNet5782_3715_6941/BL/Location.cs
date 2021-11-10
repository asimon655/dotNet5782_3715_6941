using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {

        public class Location
        {
            Location(double longitude, double lattitude)
            {
                this.Longitude = longitude;
                this.Lattitude = lattitude;
            }
            public double Longitude { set; get; }
            public double Lattitude { set; get; }
            public String DecimalToSexagesimal(double Longitude, double Latitude) /// calacs it with the well known algorithem that we found olnline ( beacuse u didnt gave that to us ) 
            {
                String result = "";
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

                // Latitude
                direction = (0 > Latitude);
                Latitude = Math.Abs(Latitude);
                result += Math.Floor(Latitude).ToString() + '\xb0';
                Latitude -= Math.Floor(Latitude);
                Latitude *= 60;
                result += Math.Floor(Latitude).ToString() + '`';
                Latitude -= Math.Floor(Latitude);
                Latitude *= 60;
                result += Math.Round(Latitude, 4).ToString() + "``";
                result += (direction ? 'E' : 'W');

                return result;
            }
        }
    }
}


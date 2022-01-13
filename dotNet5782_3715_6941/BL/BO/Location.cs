using System;

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

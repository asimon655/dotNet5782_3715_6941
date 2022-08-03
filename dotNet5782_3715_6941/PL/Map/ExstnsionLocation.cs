using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Map
{
    internal static  class ExstnsionLocation
    {
        #region Constans 
        private const double Radius = 6378137;
        private const double E = 0.0000000848191908426;
        private const double D2R = Math.PI / 180;
        private const double PiDiv4 = Math.PI / 4;
        #endregion

        public static Mapsui.Geometries.Point ToPlPoint(this  BO.Location loct ) {


            var lonRadians = D2R * loct.Longitude;
            var latRadians = D2R * loct.Lattitude;

            var x = Radius * lonRadians;
            //y=a×ln[tan(π/4+φ/2)×((1-e×sinφ)/(1+e×sinφ))^(e/2)]
            var y = Radius * Math.Log(Math.Tan(PiDiv4 + latRadians * 0.5) / Math.Pow(Math.Tan(PiDiv4 + Math.Asin(E * Math.Sin(latRadians)) / 2), E));

            return new Mapsui.Geometries.Point(x, y);


        }


        public static LocationPhoto.TileLoct ToTileLoct(this BO.Location loct ,int zoomSize )
        {
            Func<double,double> toRad = (double deg) => ((deg % 360) / 180 * Math.PI);
            LocationPhoto.TileLoct res = new LocationPhoto.TileLoct();
            int n = (int)Math.Pow(2,zoomSize);
            res.x = (int)(n * ((loct.Longitude+ 180) / 360));
            res.y= (int)(n * (1 - (Math.Log(Math.Tan(toRad(loct.Lattitude)) + 1/Math.Cos(toRad(loct.Lattitude))) / Math.PI)) / 2);
            res.z = zoomSize; 
            
            return res ; 


        }



    }
}

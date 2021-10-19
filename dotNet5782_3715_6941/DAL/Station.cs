using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public struct Station
        {
            public int Id { set; get; }
            public int Name { set; get; }
            public double Latitude { set; get; }
            public double Longitude { set; get; }
            public int ChargeSlots { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Name: " + this.Name.ToString() + " Latitude: " + this.Latitude.ToString() + " Longitude:" + this.Longitude.ToString() + " ChargeSlots: " + this.ChargeSlots.ToString();
            }
        }

    }
}

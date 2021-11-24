using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToDrone 
        {
            public int Id { set; get; }
            public double BatteryStat { set; get; }
            public Location Loct { set; get; }

            public override string ToString()
            {
                return $"Id : {Id}\n" +
                       $"location : {Loct}\n" +
                       $"battary : {BatteryStat}";
            }
        }
    } 
}

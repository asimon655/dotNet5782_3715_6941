using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToDrone : OverrideToString
        {
            public int Id { set; get; }
            public double BatteryStat { set; get; }
            public Location Loct { set; get; }
        }
    } 
}

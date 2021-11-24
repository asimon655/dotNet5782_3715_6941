using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneInCharge 
        {
            public int id { set; get;  }
            public double BatteryStat { set; get;  }

            public override string ToString()
            {
                return $"Id : {id}\n" +
                       $"battery : {BatteryStat}\n";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneToList
        {
            public int id { set; get; }
            public string Model { set; get; }
            public WeightCategories Weight { set; get; }
            public double BatteryStat { set; get; }
            public DroneStatuses DroneStat { set; get;  }
            public Location Current { set; get;  }
            public int? NumOfParcels { set; get; } = null; 


        }
    }
}

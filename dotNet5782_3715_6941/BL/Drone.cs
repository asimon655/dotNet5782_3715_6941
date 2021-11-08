using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Drone
        {
            public int Id { set; get;  }
            public String Model { set; get; }
            public WeightCategories Weight { set; get; }
            public double BatteryStat { set; get;  }
            public DroneStatuses DroneStat { set; get;  }
            public IBL.BO.ParcelInTransfer ParcelTransfer { set; get;  }
            public Location Current { set; get;  }

        }
    }
}

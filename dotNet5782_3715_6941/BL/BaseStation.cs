using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BaseStation
        {
            public int Id { set; get; }
            public int Name { set; get; }
            public Location LoctConstant { set; get; }
            public int NumOfFreeOnes { set; get; }
            public List<IBL.BO.DroneInCharge> DroneInChargeList { set ; get; }
            
        }
    }
}

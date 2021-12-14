using System.Collections.Generic;

namespace BO
{
    public class BaseStation 
    {
        public int Id { set; get; }
        public int Name { set; get; }
        public Location LoctConstant { get; set;  }
        public int NumOfFreeOnes { set; get; }
        public List<DroneInCharge> DroneInChargeList { set ; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Name : {Name}\n" +
                    $"location : {LoctConstant}\n" +
                    $"free charging slots : {NumOfFreeOnes}\n" +
                    $"drones in charge : {string.Join('\n', DroneInChargeList)}";
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public struct Drone
        {
            public int Id { set; get; }
            public String Modle { set; get; }
            public WeightCategories MaxWeigth { set; get; }
            public DroneStatuses Status { set; get; }
            public double Battery { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Model: " + this.Modle + " MaxWeight: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>(this.MaxWeigth) + " Battery:" + this.Battery.ToString() + " Status: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.DroneStatuses>((this.Status));
            }
        }

    }
}

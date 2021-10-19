using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public struct Parcel
        {
            public int Id { set; get; }
            public int SenderId { set; get; }
            public int TargetId { set; get; }
            public WeightCategories Weight { set; get; }
            public Priorities Priority { set; get; }
            public DateTime Requested { set; get; }
            public int DroneId { set; get; }
            public DateTime Schedulded { set; get; }
            public DateTime PickedUp { set; get; }
            public DateTime Delivered { set; get; }

            public override string ToString()
            {
                return "Id: " + Id.ToString() + " SenderId: " + SenderId.ToString() + " TargetId: " + TargetId.ToString() + " Weight: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>(this.Weight) + " Priorty: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Priorities>(this.Priority) + " Requested: " + Requested.ToString() + "Sechuled: " + Schedulded.ToString() + " PickedUp: " + this.PickedUp.ToString() + " Delivered " + Delivered.ToString() + " DroneId: " + DroneId.ToString();
            }
        }
    }
}

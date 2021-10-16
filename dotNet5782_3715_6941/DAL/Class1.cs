using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
   
    namespace DO
    {
        
        public struct Costumer
        {
            
            public int Id { set; get; }
            public string Name { set; get; }
            public string Phone { set; get; }
            public double Longitude { set; get; }
            public double Lattitude { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Name: " + this.Name + " Phone: " + Phone + " Latitude: " + this.Lattitude.ToString() + " Longitude:" + this.Longitude.ToString();
            }
        }

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
                return "Id: " + Id.ToString() + " SenderId: " + SenderId.ToString() + " TargetId: " + TargetId.ToString() + " Weight: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>(this.Weight) + " Priorty: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Priorities>(this.Priority) + " Requested: " + Requested.ToString() +"Sechuled: "+Schedulded.ToString()+" PickedUp: "+this.PickedUp.ToString()+" Delivered "+Delivered.ToString()+ " DroneId: " + DroneId.ToString();
            }
        }
        public  struct Drone
        {
            public int id {
                set;
                get;
            }  
            public String Modle {set; get;}
            public WeightCategories  MaxWeigth {set ; get;}
            public DroneStatuses Status { set; get;  }
            public double Battery { set; get; }

            public override string ToString()
            {
                return  "ID: " + this.id.ToString() + " Model: " + this.Modle + " MaxWeight: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>(this.MaxWeigth) + " Battery:" + this.Battery.ToString() + " Status: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.DroneStatuses>((this.Status));
            }
        }
        public struct  Staion 
        { 
               public int Id { set; get;  }
               public int Name { set; get;  }
               public double Latitude { set; get;  }
               public double Longitude { set; get; }
               public int ChargeSlots { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Name: " + this.Name.ToString() + " Latitude: " + this.Latitude.ToString() + " Longitude:" + this.Longitude.ToString() + " ChargeSlots: " + this.ChargeSlots.ToString();
            }
        } 
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StaionId{ get; set; }

            public override string ToString()
            {
                return "DroneId: " + DroneId.ToString() + " StaionId: " + StaionId.ToString();
            }
        }
        
    }
}

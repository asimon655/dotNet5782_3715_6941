using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
   
    namespace DO
    {
        struct Costumer
        {
            
            public int Id { set; get; }
            public string Name { set; get; }
            public string Phone { set; get; }
            public double Longitude { set; get; }
            public double Lattitude { set; get; }
        }

        struct Parcel
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
        }
        struct Drone
        {
            public int id {
                set => id = value;
                get => id;
            } //suagar synthetic - one time example for the other parthner . 
            public String Modle {set; get;}
            public WeightCategories  MaxWeigth {set ; get;}
            public DroneStatuses Status { set; get;  }
            public double Battery { set; get; }

        }
        struct Staion
        { 
               public int Id { set; get;  }
               public int Name { set; get;  }
               public double Latitude { set; get;  }
               public double Longitude { set; get }
               public int ChargeSlots { set; get; }
        } 
        struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StaionId{ get; set; }
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DalObject
    { 
        class DataSource
        {
            internal static IDAL.DO.Drone[] Drones =new  IDAL.DO.Drone[10];
            internal static IDAL.DO.Costumer [] Costumers= new IDAL.DO.Costumer[100] ;
            internal static IDAL.DO.Staion[] Staions = new IDAL.DO.Staion[5];
            internal static IDAL.DO.Parcel[] Parcels = new IDAL.DO.Parcel[1000];
            //until here array var declartion 
            internal class Config
            {

                static internal int DronesFirst = 0;
                static internal int CostumerFirst = 0;
                static internal int StaionsFirst = 0;
                static internal int ParcelFirst = 0;
                static internal int idcreation = 0;
                static internal void Initalize() {
                    CostumerFirst = 10;
                    DronesFirst = 5;
                    StaionsFirst = 2;
                    ParcelFirst = 10;
                    for (int i = 0; i < 2; i++)
                    {
                       
                        Staions[i] = new IDAL.DO.Staion();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                       
                        Costumers[i] = new IDAL.DO.Costumer();
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        
                        Drones[i] = new IDAL.DO.Drone();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Parcels[i] = new IDAL.DO.Parcel();
                    }





                } 
            
            
            }

           
        
        
        }
    
    
    
    } 
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

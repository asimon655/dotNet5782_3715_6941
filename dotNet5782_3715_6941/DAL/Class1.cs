using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        struct Drone
        {
            public int id {
                set => id = value;
                get => id;
            }
            public String Modle
            {
                set; get;

            }
            public WeightCategories  MaxWeigth{
                set ; get;

                }

            public DroneStatuses Status
            { set; get;  }
            public double Battery {
                set; get; 
            
            }

        }
        struct Staion
        { 

               public int Id { set; get;  }
                public int Name { set; get;  }
                public double Latitude { set; get;  }
                public double Longitude { set; get; }

                public int ChargeSlots { set; get; }

        } 
        struct DroneCharge
        {
            public int DroneId { get; set; }

            public int StaionId{ get; set; } 
        
        
        
        
        }
        
    }
}

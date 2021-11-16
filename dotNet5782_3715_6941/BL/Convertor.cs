using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    namespace Convertor
    {
        public interface DOTOBO
        {
            Drone DronesC(IDAL.DO.Drone drone)
            { 
                return new Drone () { Id = drone.Id, Weight = (WeightCategories)drone.MaxWeigth, Model = drone.Modle   };

            }
           
        
        }

    }
}

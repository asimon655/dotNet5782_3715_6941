using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
_________ ______            _______
\__   __/(  __  \ |\     /|(       )
   ) (   | (  \  )| )   ( || () () |
   | |   | |   ) || (___) || || || |
   | |   | |   | ||  ___  || |(_)| |
   | |   | |   ) || (   ) || |   | |
___) (___| (__/  )| )   ( || )   ( |
\_______/(______/ |/     \||/     \|



 
 _______  _______ _________ _______  _______  _
(  ___  )(  ____ \\__   __/(       )(  ___  )( (    /|
| (   ) || (    \/   ) (   | () () || (   ) ||  \  ( |
| (___) || (_____    | |   | || || || |   | ||   \ | |
|  ___  |(_____  )   | |   | |(_)| || |   | || (\ \) |
| (   ) |      ) |   | |   | |   | || |   | || | \   |
| )   ( |/\____) |___) (___| )   ( || (___) || )  \  |
|/     \|\_______)\_______/|/     \|(_______)|/    )_)


 
 */
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
                return "ID: " + this.Id.ToString() + " Model: " + this.Modle + " MaxWeight: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>(this.MaxWeigth) + " Battery:" + this.Battery.ToString() + " Status: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.DroneStatuses>((this.Status)); /// returns strings with all the args of the struct in string  
            }
        }

    }
}

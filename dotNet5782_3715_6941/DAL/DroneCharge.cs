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
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StaionId { get; set; }

            public override string ToString()
            {
                return "DroneId: " + DroneId.ToString() + " StaionId: " + StaionId.ToString(); /// returns strings with all the args of the struct in string 
            }
        }
    }
}


namespace DAL
{
    namespace DalObject
    {
        public partial class DalObject : IDAL.Idal
        {

            public void BindDroneAndStaion(int DroneId, int staionId)
            {
                IDAL.DO.Station Origin = PullDataStation(staionId);
                if (Origin.ChargeSlots > 0)
                {
                    IDAL.DO.DroneCharge DroneChargeTmp = new IDAL.DO.DroneCharge() { StaionId = staionId, DroneId = DroneId };
                    DataSource.DronesCharges.Add(DroneChargeTmp);
                    Origin.ChargeSlots -= 1;
                    Update<IDAL.DO.Station>(DataSource.Stations, Origin);
                }
                else
                    throw new NotImplementedException();


            }


        }
    }
}
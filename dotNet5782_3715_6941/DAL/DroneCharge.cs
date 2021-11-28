using IDAL.DO;
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


namespace DalObject
{
    public partial class DalObject : IDAL.Idal
    {
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            // if we find that the drone is already in charging
            if (DataSource.DronesCharges.Any(s => s.DroneId == droneCharge.DroneId))
            {
                throw new IdAlreadyExists("the Drone is already in charging", droneCharge.DroneId);
            }

            DataSource.DronesCharges.Add(droneCharge);
        }

        public DroneCharge PullDataDroneChargeByDroneId(int droneId)
        {
            DroneCharge droneCharge = DataSource.DronesCharges.Find(s => s.DroneId == droneId);
            /// if the Drone wasnt found throw error
            if (droneCharge.DroneId != droneId)
            {
                throw new IdDosntExists("the droneId could not be found", droneId);
            }
            return droneCharge;
        }

        public IEnumerable<DroneCharge> DronesChargesPrint()
        {
            return DataSource.DronesCharges;
        }

        public void UpdateDroneCharge(DroneCharge droneCharge)
        {
            // if we cant find that the id we throw error
            if (!DataSource.DronesCharges.Any(s => s.DroneId == droneCharge.DroneId))
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", droneCharge.DroneId);
            }
            Update<DroneCharge>(DataSource.DronesCharges, droneCharge);
        }

        public void DeleteDroneCharge(int droneId)
        {
            // if we cant find that the id we throw error
            if (DataSource.DronesCharges.RemoveAll(s => s.DroneId == droneId) < 1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", droneId);
            }
        }
    }
}

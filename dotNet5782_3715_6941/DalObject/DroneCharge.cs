using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    public partial class DalObject : DalApi.IDal
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
        public IEnumerable<DroneCharge> GetDronesCharges(Predicate<DroneCharge> expr) {
            return DataSource.DronesCharges.FindAll(expr);
        }
        public int CountDronesCharges(Func<DroneCharge, bool> expr)
        {
            return DataSource.DronesCharges.Count(expr);
        }
    }
}

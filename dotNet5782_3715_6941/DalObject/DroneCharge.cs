using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            // if we find that the drone is already in charging
            if (DataSource.DronesCharges.Any(s => s.DroneId == droneCharge.DroneId))
            {
                throw new IdAlreadyExists("the Drone is already in charging", droneCharge.DroneId);
            }

            DataSource.DronesCharges.Add(droneCharge);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneId)
        {
            DroneCharge droneCharge = DataSource.DronesCharges.Find(s => s.DroneId == droneId);
            /// if the Drone wasnt found throw error
            if (droneCharge.DroneId != droneId)
            {
                throw new IdDosntExists("the droneId could not be found", droneId);
            }
            return droneCharge;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDronesCharges()
        {
            return DataSource.DronesCharges;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int droneId)
        {
            // if we cant find that the id we throw error
            if (DataSource.DronesCharges.RemoveAll(s => s.DroneId == droneId) < 1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", droneId);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDronesCharges(Predicate<DroneCharge> expr)
        {
            return DataSource.DronesCharges.FindAll(expr);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CountDronesCharges(Func<DroneCharge, bool> expr)
        {
            return DataSource.DronesCharges.Count(expr);
        }
    }
}

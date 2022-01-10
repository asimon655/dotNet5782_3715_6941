using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> data = Read<DroneCharge>();

            if (data.Any(x => x.DroneId == droneCharge.DroneId))
                throw new IdAlreadyExists("the drone is already in charge", droneCharge.DroneId);

            data.Add(droneCharge);

            Write(data);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneId)
        {
            List<DroneCharge> dronesCharges = Read<DroneCharge>();

            DroneCharge droneCharge = dronesCharges.Find(s => s.DroneId == droneId);

            /// if the Drone wasnt found throw error
            if (droneCharge.DroneId != droneId)
            {
                throw new IdDosntExists("the Id could not be found", droneId);
            }
            return droneCharge;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDronesCharges()
        {
            return Read<DroneCharge>();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDronesCharges(Predicate<DroneCharge> expr)
        {
            return Read<DroneCharge>().FindAll(expr);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CountDronesCharges(Func<DroneCharge, bool> expr)
        {
            return Read<DroneCharge>().Count(expr);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int droneId)
        {
            List<DroneCharge> data = Read<DroneCharge>();

            int removed = data.RemoveAll(x => x.DroneId == droneId);

            if (removed == 0)
                throw new IdDosntExists("the drone is not in charge", droneId);

            Write(data);
        }
    }
}

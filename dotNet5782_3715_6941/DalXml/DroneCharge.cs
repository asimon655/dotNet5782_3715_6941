using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> data = Read<DroneCharge>();

            if (data.Any(x => x.DroneId == droneCharge.DroneId))
                throw new IdAlreadyExists("the drone is already in charge", droneCharge.DroneId);

            data.Add(droneCharge);

            Write(data);
        }

        public DroneCharge GetDroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetDronesCharges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetDronesCharges(Predicate<DroneCharge> expr)
        {
            throw new NotImplementedException();
        }

        public int CountDronesCharges(Func<DroneCharge, bool> expr)
        {
            throw new NotImplementedException();
        }

        public void DeleteDroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }
    }
}

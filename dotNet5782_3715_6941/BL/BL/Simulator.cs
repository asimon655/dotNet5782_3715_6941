using System;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public void StartSimulator(int droneId, Action refresh, Func<bool> stop)
        {
            new Simulator.Simulator(Instance, droneId, refresh, stop);
        }
        public void SimulatorUpdateBattary(int droneId, double battary)
        {
            GetDroneToList(droneId).Battery = battary;
        }
        public void SimulatorUpdateLocation(int droneId, BO.Location location)
        {
            GetDroneToList(droneId).Loct = location;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.Bl;

namespace Simulator
{
    class Simulator
    {
        const int delay = 1000;
        
        // speed in km/s
        const double speed = 5;

        public Simulator(BlApi.Ibl logic, int droneId, Action refresh, Func<bool> stop)
        {
            // GetDroneToList throw IdDosnt Exsist if needed
            //DroneList drone = logic.GetDroneToList(droneId);

            while (stop())
            {
                Drone drone = logic.GetDrone(droneId);

                switch (drone.DroneStat)
                {
                    case DroneStatuses.Free:
                        try
                        {
                            logic.BindParcelToDrone(droneId);
                            refresh();
                        }
                        catch (CouldntFindPatcelThatsFits)
                        {
                            logic.DroneCharge(droneId);
                            refresh();
                        }
                        catch (IdDosntExists) { }
                        break;
                    case DroneStatuses.Delivery:
                        try
                        {
                            Parcel parcel = logic.GetParcel((int)drone.ParcelTransfer.Id);

                            if (ParcelStatusC(parcel) == ParcelStatus.Binded)
                            {
                                Thread.Sleep(delay);
                                logic.DronePickUp(droneId);
                                refresh();
                            }
                            else if (ParcelStatusC(parcel) == ParcelStatus.PickedUp)
                            {
                                Thread.Sleep(delay);
                                logic.DroneDelivere(droneId);
                                refresh();
                            }
                        }
                        catch (IdDosntExists) { }
                        break;
                    case DroneStatuses.Matance:
                        try
                        {
                            double percenageGap = 100 - drone.BatteryStat;
                            double chargingPeriod = percenageGap * ChargingSpeed;
                            // multiply chargingPeriod in 3600 to make it sec from hours
                            Thread.Sleep(delay);
                            logic.DroneReleaseCharge(droneId, chargingPeriod);
                            refresh();
                        }
                        catch (IdDosntExists) { }
                        break;
                }
            }
        }
    }
}

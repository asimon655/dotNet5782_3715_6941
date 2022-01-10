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
        const int timeScale = 500;
        
        // speed in km/s
        const double speed = 0.01;

        public Simulator(BlApi.Ibl logic, int droneId, Action refresh, Func<bool> stop)
        {
            // GetDroneToList throw IdDosnt Exsist if needed
            DroneList drone = logic.GetDroneToList(droneId);

            while (stop())
            {
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
                            Parcel parcel = logic.GetParcel((int)drone.ParcelId);
                            Drone drn = logic.Convert(drone);

                            if (logic.ParcelStatusC(parcel) == ParcelStatus.Binded)
                            {
                                logic.DronePickUp(droneId);
                                refresh();

                                Thread.Sleep((int)(drn.ParcelTransfer.Distance * speed * timeScale));
                            }
                            else if (logic.ParcelStatusC(parcel) == ParcelStatus.PickedUp)
                            {
                                logic.DroneDelivere(droneId);
                            refresh();

                                Thread.Sleep((int)(drn.ParcelTransfer.Distance * speed * timeScale));
                            }
                        }
                        catch (IdDosntExists) { }
                        break;
                    case DroneStatuses.Matance:
                        try
                        {
                            double percenageGap = 100 - drone.Battery;
                            double chargingPeriod = percenageGap * logic.GetChargingSpeed();
                            // multiply chargingPeriod in 3600 to make it sec from hours
                            Thread.Sleep((int)(timeScale * 3600 * chargingPeriod));
                            logic.DroneReleaseCharge(droneId, chargingPeriod);
                        }
                        catch (IdDosntExists) { }
                        break;
                }
            }
        }
    }
}

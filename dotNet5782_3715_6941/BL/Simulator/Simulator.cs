using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.Bl;
using Itinero.LocalGeo;

namespace Simulator
{
    class Simulator
    {
        const int delay = 300;
        
        // speed in km/s
        const double speed = 0.3 ;

        BlApi.Ibl logic;
        Action refresh;

        public Simulator(BlApi.Ibl _logic, int droneId, Action _refresh, Func<bool> stop)
        {
            logic = _logic;
            refresh = _refresh;

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
                        catch (notEnoughBattery)
                        {
                            int stationId = logic.SimulatorDroneSaveChargeSlot(droneId);
                            Location stationLoct = logic.GetStation(stationId).LoctConstant;
                            moveDroneTo(drone, stationLoct);
                            logic.SimulatorDroneReleaseChargeSlot(droneId);
                            logic.DroneCharge(droneId);
                            refresh();
                        }
                        catch (CouldntFindPatcelThatsFits)
                        {
                            Thread.Sleep(delay);
                        }
                        catch (IdDosntExists) { }
                        break;
                    case DroneStatuses.Delivery:
                        try
                        {
                            Parcel parcel = logic.GetParcel(drone.ParcelTransfer.Id);

                            if (ParcelStatusC(parcel) == ParcelStatus.Binded)
                            {
                                moveDroneTo(drone, drone.ParcelTransfer.Pickup);
                                logic.DronePickUp(droneId);
                                refresh();
                            }
                            else if (ParcelStatusC(parcel) == ParcelStatus.PickedUp)
                            {
                                moveDroneTo(drone, drone.ParcelTransfer.Dst);
                                logic.DroneDelivere(droneId);
                                refresh();
                            }
                        }
                        catch (IdDosntExists) { }
                        break;
                    case DroneStatuses.Matance:
                        try
                        {
                            charge(drone);
                            double percenageGap = 100 - drone.BatteryStat;
                            double chargingPeriod = percenageGap * ChargingSpeed;
                            // multiply chargingPeriod in 3600 to make it sec from hours
                            logic.DroneReleaseCharge(droneId, chargingPeriod / 3600);
                            refresh();
                        }
                        catch (IdDosntExists) { }
                        break;
                }
            }
        }

        void moveDroneTo(Drone drone, Location location, WeightCategories? weightCategories = null)
        {
            Coordinate source = new Coordinate(drone.Current.Lattitude, drone.Current.Longitude);
            Coordinate destination = new Coordinate(location.Lattitude, location.Longitude);

            Line line = new Line(source, destination);

            double totalDistance = Coordinate.DistanceEstimateInMeter(source, destination) / 1000;

            double powerUsageForSpeed = getPowerUsage(speed, weightCategories);

            double traveledDistance = 0;
            while (traveledDistance < (totalDistance - speed))
            {
                traveledDistance += speed;

                drone.Current = new Location(line.LocationAfterDistance((float)traveledDistance * 1000));
                drone.BatteryStat -= powerUsageForSpeed;

                logic.SimulatorUpdateLocation(drone.Id, drone.Current);
                logic.SimulatorUpdateBattary(drone.Id, drone.BatteryStat);
                
                refresh();
                
                Thread.Sleep(delay);
            }
        }

        void charge(Drone drone)
        {
            while (drone.BatteryStat < 100 - ChargingSpeed)
            {
                drone.BatteryStat += ChargingSpeed;

                logic.SimulatorUpdateBattary(drone.Id, drone.BatteryStat);

                refresh();

                Thread.Sleep(delay);
            }
        }
    }
}

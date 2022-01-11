﻿using System;
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
        const int delay = 1000;
        
        // speed in km/s
        const double speed = 10;

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
    }
}

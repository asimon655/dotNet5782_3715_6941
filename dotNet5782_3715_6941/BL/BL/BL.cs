using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        static Bl() {}
        internal static readonly object padlock = new object();
        internal static Bl instance;
        internal static Bl Instance {
            get {
                if (instance == null) {
                    lock(padlock) {  
                        if (instance == null) {  
                            instance = new Bl();  
                        }
                    }
                }
                return instance;  
            }
        }



        DalApi.IDal data;

        List<DroneList> drones = new List<DroneList>();

        internal Random RandomGen = new Random();

        double PowerConsumptionFree;
        double PowerConsumptionLight;
        double PowerConsumptionMedium;
        double PowerConsumptionHeavy;
        double ChargingSpeed;

        public Bl()
        {
            data = DalApi.DalFactory.GetDal();

            // initilazing power related consts
            double[] powerConst = data.GetPowerConsumption();

            PowerConsumptionFree = powerConst[0];
            PowerConsumptionLight = powerConst[1];
            PowerConsumptionMedium = powerConst[2];
            PowerConsumptionHeavy = powerConst[3];
            ChargingSpeed = powerConst[4];

            // initilazing the drones list
            foreach (var drone in data.GetDrones())
            {
                DroneList newDrone = new DroneList();
                newDrone.Id = drone.Id;
                newDrone.Model = drone.Modle;
                newDrone.Weight = (WeightCategories)drone.MaxWeigth;

                int parcelId = getBindedUndeliveredParcel(drone.Id);

                if (parcelId != -1) // the drone is binded
                {
                    newDrone.DroneStat = DroneStatuses.Delivery;
                    newDrone.ParcelId = parcelId;
                    // get the binded parcel
                    DO.Parcel parcel = data.GetParcel(parcelId);

                    // get the sender and target and there location
                    DO.Customer sender = data.GetCustomer(parcel.SenderId);
                    Location senderLoct = new Location(sender.Longitude, sender.Lattitude);
                    DO.Customer target = data.GetCustomer(parcel.TargetId);
                    Location targetLoct = new Location(target.Longitude, target.Lattitude);

                    // the parcel has been binded but not picked up yet
                    if (ParcelStatusC(parcel) == ParcelStatus.Binded)
                    {
                        // set the location of the drone to the closest station to the sender
                        int senderStationId = getClosesStation(senderLoct);
                        DO.Station senderStation = data.GetStation(senderStationId);
                        Location senderStationLoct = new Location(senderStation.Longitude, senderStation.Lattitude);
                        newDrone.Loct = senderStationLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        DO.Station targetStation = data.GetStation(targetStationId);
                        Location targetStationLoct = new Location(targetStation.Longitude, targetStation.Lattitude);
                        // calculate the minimum battery this trip will take
                        double minimumBattery = 0;
                        minimumBattery += getPowerUsage(newDrone.Loct, senderLoct);
                        minimumBattery += getPowerUsage(senderLoct, targetLoct, (WeightCategories?)parcel.Weight);
                        minimumBattery += getPowerUsage(targetLoct, targetStationLoct);
                        // set the drone battery randomly between the minimumBattery and 100%
                        newDrone.Battery = 100D - RandomGen.NextDouble() * minimumBattery;
                    }
                    // the parcel has been binded and picked up but hasnt been deliverd yet
                    else
                    {
                        // set the location of the drone to the location of the sender
                        newDrone.Loct = senderLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        DO.Station targetStation = data.GetStation(targetStationId);
                        Location targetStationLoct = new Location(targetStation.Longitude, targetStation.Lattitude);
                        // calculate the minimum battery this trip will take
                        double minimumBattery = 0;
                        minimumBattery += getPowerUsage(newDrone.Loct, targetLoct, (WeightCategories?)parcel.Weight);
                        minimumBattery += getPowerUsage(targetLoct, targetStationLoct);
                        // set the drone battery randomly between the minimumBattery and 100%
                        newDrone.Battery = 100D - RandomGen.NextDouble() * minimumBattery;
                    }
                }
                else // the drone is not binded
                {
                    // set DroneStat to a random value between Free, Matance
                    newDrone.DroneStat = (DroneStatuses)RandomGen.Next((int)DroneStatuses.Free, (int)DroneStatuses.Matance);
                }
                if (newDrone.DroneStat == DroneStatuses.Matance)
                {
                    // set drone location to a random starion with free charging slots
                    IEnumerable<DO.Station> stationsFreePorts = data.GetStations(x => x.ChargeSlots > 0);
                    int random_i = RandomGen.Next(stationsFreePorts.Count());
                    DO.Station station = stationsFreePorts.ElementAt(random_i);
                    newDrone.Loct = new Location(station.Longitude, station.Lattitude);
                    // register the matance in drone charge
                    data.AddDroneCharge(new DO.DroneCharge { StaionId = station.Id, DroneId = newDrone.Id });
                    // set drone battery
                    newDrone.Battery = RandomGen.NextDouble() * 20;
                }
                if (newDrone.DroneStat == DroneStatuses.Free)
                {
                    // set drone location to a random customer that recived a parcel
                    IEnumerable<DO.Parcel> parcelsDelivered = data.GetParcels(x => ParcelStatusC(x) == ParcelStatus.Delivered);
                    if (parcelsDelivered.Count() > 0)
                    {
                        int random_i = RandomGen.Next(parcelsDelivered.Count());
                        DO.Parcel parcel = parcelsDelivered.ElementAt(random_i);
                        DO.Customer costumer = data.GetCustomer(parcel.TargetId);
                        newDrone.Loct = new Location(costumer.Longitude, costumer.Lattitude);
                    }
                    // if there are no deliverd parcels 
                    else
                    {
                        // set drone location to a random starion with free charging slots
                        IEnumerable<DO.Station> stationsFreePorts = data.GetStations(x => x.ChargeSlots > 0);
                        int random_i = RandomGen.Next(stationsFreePorts.Count());
                        DO.Station stationy = stationsFreePorts.ElementAt(random_i);
                        newDrone.Loct = new Location(stationy.Longitude, stationy.Lattitude);
                    }

                    // get the location of the closest station to that costumer
                    int stationId = getClosesStation(newDrone.Loct);
                    DO.Station station = data.GetStation(stationId);
                    Location stationLoct = new Location(station.Longitude, station.Lattitude);

                    // calculate the minimum battery this trip will take
                    double minimumBattery = 0;
                    minimumBattery += getPowerUsage(newDrone.Loct, stationLoct);
                    // set the drone battery randomly between the minimumBattery and 100%
                    newDrone.Battery = 100D - RandomGen.NextDouble() * minimumBattery;
                }
                // add newDrone to drones list
                drones.Add(newDrone);
            }
        }
        public double GetChargingSpeed() => ChargingSpeed;
    }
}

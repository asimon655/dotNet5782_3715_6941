using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class Bl : IBL.Ibl
    {
        DalApi.IDal data;

        List<DroneToList> drones = new List<DroneToList>();

        internal Random RandomGen = new Random();

        double PowerConsumptionFree;
        double PowerConsumptionLight;
        double PowerConsumptionMedium;
        double PowerConsumptionHeavy;
        double ChargingSpeed;

        public Bl()
        {
            try
            {
                data = DalFactory.DalFactory.GetDal("DalObject");
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }
            // initilazing power related consts
            double[] powerConst = data.GetPowerConsumption();

            PowerConsumptionFree = powerConst[0];
            PowerConsumptionLight = powerConst[1];
            PowerConsumptionMedium = powerConst[2];
            PowerConsumptionHeavy = powerConst[3];
            ChargingSpeed = powerConst[4];

            // initilazing the drones list
            foreach (var drone in data.DronesPrint())
            {
                DroneToList newDrone = new DroneToList();
                newDrone.Id = drone.Id;
                newDrone.Model = drone.Modle;
                newDrone.Weight = (WeightCategories)drone.MaxWeigth;

                int parcelId = getBindedUndeliveredParcel(drone.Id);

                if (parcelId != -1) // the drone is binded
                {
                    newDrone.DroneStat = DroneStatuses.Delivery;
                    newDrone.ParcelIdTransfer = parcelId;
                    // get the binded parcel
                    DO.Parcel parcel = data.PullDataParcel(parcelId);

                    // get the sender and target and there location
                    DO.Costumer sender = data.PullDataCostumer(parcel.SenderId);
                    Location senderLoct = new Location(sender.Longitude, sender.Lattitude);
                    DO.Costumer target = data.PullDataCostumer(parcel.TargetId);
                    Location targetLoct = new Location(target.Longitude, target.Lattitude);

                    // the parcel has been binded but not picked up yet
                    if (ParcelStatC(parcel) == ParcelStat.Binded)
                    {
                        // set the location of the drone to the closest station to the sender
                        int senderStationId = getClosesStation(senderLoct);
                        DO.Station senderStation = data.PullDataStation(senderStationId);
                        Location senderStationLoct = new Location(senderStation.Longitude, senderStation.Lattitude);
                        newDrone.Current = senderStationLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        DO.Station targetStation = data.PullDataStation(targetStationId);
                        Location targetStationLoct = new Location(targetStation.Longitude, targetStation.Lattitude);
                        // calculate the minimum battery this trip will take
                        double minimumBattery = 0;
                        minimumBattery += getPowerUsage(newDrone.Current, senderLoct);
                        minimumBattery += getPowerUsage(senderLoct, targetLoct, (WeightCategories?)parcel.Weight);
                        minimumBattery += getPowerUsage(targetLoct, targetStationLoct);
                        // set the drone battery randomly between the minimumBattery and 100%
                        newDrone.BatteryStat = 100D - RandomGen.NextDouble() * minimumBattery;
                    }
                    // the parcel has been binded and picked up but hasnt been deliverd yet
                    else
                    {
                        // set the location of the drone to the location of the sender
                        newDrone.Current = senderLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        DO.Station targetStation = data.PullDataStation(targetStationId);
                        Location targetStationLoct = new Location(targetStation.Longitude, targetStation.Lattitude);
                        // calculate the minimum battery this trip will take
                        double minimumBattery = 0;
                        minimumBattery += getPowerUsage(newDrone.Current, targetLoct, (WeightCategories?)parcel.Weight);
                        minimumBattery += getPowerUsage(targetLoct, targetStationLoct);
                        // set the drone battery randomly between the minimumBattery and 100%
                        newDrone.BatteryStat = 100D - RandomGen.NextDouble() * minimumBattery;
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
                    newDrone.Current = new Location(station.Longitude, station.Lattitude);
                    // register the matance in drone charge
                    data.AddDroneCharge(new DO.DroneCharge { StaionId = station.Id, DroneId = newDrone.Id });
                    // set drone battery
                    newDrone.BatteryStat = RandomGen.NextDouble() * 20;
                }
                if (newDrone.DroneStat == DroneStatuses.Free)
                {
                    // set drone location to a random customer that recived a parcel
                    IEnumerable<DO.Parcel> parcelsDelivered = data.GetParcels(x => ParcelStatC(x) == ParcelStat.Delivered);
                    int random_i = RandomGen.Next(parcelsDelivered.Count());
                    DO.Parcel parcel = parcelsDelivered.ElementAt(random_i);
                    DO.Costumer costumer = data.PullDataCostumer(parcel.TargetId);
                    newDrone.Current = new Location(costumer.Longitude, costumer.Lattitude);

                    // get the location of the closest station to that costumer
                    int stationId = getClosesStation(newDrone.Current);
                    DO.Station station = data.PullDataStation(stationId);
                    Location stationLoct = new Location(station.Longitude, station.Lattitude);

                    // calculate the minimum battery this trip will take
                    double minimumBattery = 0;
                    minimumBattery += getPowerUsage(newDrone.Current, stationLoct);
                    // set the drone battery randomly between the minimumBattery and 100%
                    newDrone.BatteryStat = 100D - RandomGen.NextDouble() * minimumBattery;
                }
                // add newDrone to drones list
                drones.Add(newDrone);
            }
        }


      

    }
}

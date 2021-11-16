using System;
using System.Collections.Generic;
using IBL.BO;
namespace BL
{
    public class Bl : IBL.Ibl
    {
        IDAL.Idal data = new DAL.DalObject.DalObject();

        List<DroneToList> drones = new List<DroneToList>();

        internal Random RandomGen = new Random();

        double PowerConsumptionFree;
        double PowerConsumptionLight;
        double PowerConsumptionMedium;
        double PowerConsumptionHeavy ;
        double ChargingSpeed;

        public Bl()
        {
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
                    IDAL.DO.Parcel parcel = data.PullDataParcel(parcelId);

                    // get the sender and target and there location
                    IDAL.DO.Costumer sender = data.PullDataCostumer(parcel.SenderId);
                    Location senderLoct = new Location(sender.Longitude, sender.Lattitude);
                    IDAL.DO.Costumer target = data.PullDataCostumer(parcel.TargetId);
                    Location targetLoct = new Location(target.Longitude, target.Lattitude);

                    // the parcel has been binded but not picked up yet
                    if (parcel.PickedUp == DateTime.MinValue)
                    {
                        // set the location of the drone to the closest station to the sender
                        int senderStationId = getClosesStation(senderLoct);
                        IDAL.DO.Station senderStation = data.PullDataStation(senderStationId);
                        Location senderStationLoct = new Location(senderStation.Longitude, senderStation.Lattitude);
                        newDrone.Current = senderStationLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        IDAL.DO.Station targetStation = data.PullDataStation(targetStationId);
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
                        IDAL.DO.Station targetStation = data.PullDataStation(targetStationId);
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
                    List<IDAL.DO.Station> stationsFreePorts = getStationsFreePorts();
                    IDAL.DO.Station station = stationsFreePorts[RandomGen.Next(stationsFreePorts.Count)];
                    newDrone.Current = new Location(station.Longitude, station.Lattitude);
                    // TODO: register this matance in DroneCharge
                    // set drone battery
                    newDrone.BatteryStat = RandomGen.NextDouble() * 20; 
                }
                if (newDrone.DroneStat == DroneStatuses.Free)
                {
                    // set drone location to a random customer that recived a message
                    List<IDAL.DO.Parcel> parcelsDelivered = getDeliverdParcels();
                    IDAL.DO.Parcel parcel = parcelsDelivered[RandomGen.Next(parcelsDelivered.Count)];
                    IDAL.DO.Costumer costumer = data.PullDataCostumer(parcel.TargetId);
                    newDrone.Current = new Location(costumer.Longitude, costumer.Lattitude);
                    
                    // get the location of the closest station to that costumer
                    int stationId = getClosesStation(newDrone.Current);
                    IDAL.DO.Station station = data.PullDataStation(stationId);
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
        // return the id of the parcel that binded to a specific drone and the parcel not yet delivered
        // if there isnt any return -1
        int getBindedUndeliveredParcel(int droneId)
        {
            foreach (var parcel in data.ParcelsPrint())
            {
                if (parcel.Id == droneId && parcel.Delivered == DateTime.MinValue)
                {
                    return droneId;
                }
            }
            return -1;
        }
        // calculate distance between two locations
        double calculateDistance(Location location1, Location location2) {
            double rlat1 = Math.PI * location1.Lattitude / 180;
            double rlat2 = Math.PI * location2.Lattitude / 180;
            double theta = location1.Longitude - location2.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return Math.Round(dist * 1.609344, 2);
        }
        // return the closes station to a given location
        int getClosesStation(Location location)
        {
            int stationId = 0;
            double shortestDistance = double.MaxValue;
            foreach (var station in data.StationsPrint())
            {
                double distance = calculateDistance(new Location(station.Longitude, station.Lattitude), location);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    stationId = station.Id;
                }
            }
            return stationId;
        }

        double getPowerUsage(Location from, Location to, WeightCategories? weight = null)
        {
            switch(weight)
            {
                case WeightCategories.Easy:
                    return calculateDistance(from, to) * PowerConsumptionLight;
                case WeightCategories.Medium:
                    return calculateDistance(from, to) * PowerConsumptionMedium;
                case WeightCategories.Heavy:
                    return calculateDistance(from, to) * PowerConsumptionHeavy;
                default: // the drone is free
                    return calculateDistance(from, to) * PowerConsumptionFree;
            }
        }
        // return a list of stations with free charging slots
        // (this is a help function so its useful to return list than ienumerable)
        List<IDAL.DO.Station> getStationsFreePorts()
        {
            IEnumerable<IDAL.DO.Station> stations = data.StationsPrint();
            List<IDAL.DO.Station> res = new List<IDAL.DO.Station>();
            foreach (var station in stations)
            {
                if (station.ChargeSlots > 0)
                {
                    res.Add(station);
                }
            }
            return res;
        }
        List<IDAL.DO.Parcel> getDeliverdParcels()
        {
            IEnumerable<IDAL.DO.Parcel> parcels = data.ParcelsPrint();
            List<IDAL.DO.Parcel> res = new List<IDAL.DO.Parcel>();
            foreach (var parcel in parcels)
            {
                if (parcel.Delivered != DateTime.MinValue)
                {
                    res.Add(parcel);
                }
            }
            return res;
        }
        public void AddCostumer(Costumer costumer)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(Drone drone, int stationId)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        public void AddStation(BaseStation station)
        {
            throw new NotImplementedException();
        }

        public BaseStation PullDataStaion(int id)
        {
            throw new NotImplementedException();
        }

        public Costumer PullDataCostumer(int id)
        {
            throw new NotImplementedException();
        }

        public Drone PullDataDrone(int id)
        {
            throw new NotImplementedException();
        }

        public Parcel PullDataParcel(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(int droneId, string droneName)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateCostumer(int costumerId, string costumerName = null, string costumerPhone = null)
        {
            throw new NotImplementedException();
        }

        public void BindParcelToDrone(int droneId)
        {
            throw new NotImplementedException();
        }

        public void PickUpByDrone(int droneId)
        {
            throw new NotImplementedException();
        }

        public void ParcelDeliveredToCostumer(int droneId)
        {
            throw new NotImplementedException();
        }

        public void DroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }

        public void DroneChargeRelease(int droneId, double chargingPeriod)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStaionToList> StaionsPrint()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BaseStaionToList> BaseStaionsFreePortsPrint()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClientToList> CostumersPrint()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneToList> DronesPrint()
        {
            return drones;
        }

        public IEnumerable<ParcelToList> ParcelsPrint()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParcelToList> ParcelsWithoutDronesPrint()
        {
            throw new NotImplementedException();
        }
    } 
}

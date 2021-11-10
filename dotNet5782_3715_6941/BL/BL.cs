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
                if (parcelId != -1) // the drone is not binded
                {
                    newDrone.DroneStat = DroneStatuses.Delivery;
                    // TODO: need to set drone location
                    // TODO: need to set drone battery
                }
                else // the drone is binded
                {
                    // set DroneStat to a random value between Free, Matance
                    newDrone.DroneStat = (DroneStatuses)RandomGen.Next((int)DroneStatuses.Free, (int)DroneStatuses.Matance);
                }
                if (newDrone.DroneStat == DroneStatuses.Matance)
                {
                    // TODO: need to set drone location
                    newDrone.BatteryStat = RandomGen.NextDouble() * 20; 
                }
                if (newDrone.DroneStat == DroneStatuses.Free)
                {
                    // TODO: need to set drone location
                    // TODO: need to set drone battery
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

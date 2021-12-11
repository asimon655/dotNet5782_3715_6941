using BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        // get Binded Undelivered Parcel if not found return -1
        int getBindedUndeliveredParcel(int droneId)
        {
            IEnumerable<DO.Parcel> parcels = data.GetParcels(x => x.DroneId == droneId && ParcelStatC(x) != ParcelStat.Delivered);
            DO.Parcel parcel = parcels.FirstOrDefault();
            if (parcel.DroneId != droneId)
            {
                return -1;
            }
            return parcel.Id;
        }
        // calculate distance between two locations


        double calculateDistance(Location location1, Location location2)
        {
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
            switch (weight)
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


        private Location getParcelLoctSender(DO.Parcel parcel)
        {
            try
            {
                DO.Costumer CSMtmp = data.PullDataCostumer(parcel.SenderId);
                return new Location(CSMtmp.Longitude, CSMtmp.Lattitude);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);

            }


        }

        private Location getParcelLoctTarget(DO.Parcel parcel)
        {
            try
            {
                DO.Costumer CSMtmp = data.PullDataCostumer(parcel.TargetId);
                return CostumerC(CSMtmp).Loct;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);

            }



        }



        private bool canreach(DroneToList drony, DO.Parcel parcel, Func<DO.Parcel, Location> function)
            => getPowerUsage(drony.Current, function(parcel), (WeightCategories)parcel.Weight) <= drony.BatteryStat;


        private DO.Station GetStationFromCharging(int droneId)
        {

            try
            {
                return data.PullDataStation(data.PullDataDroneChargeByDroneId(droneId).StaionId);

            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }



        }

        private DroneToList GetDroneToList(int Id)
        {
            DroneToList drone = drones.Find(s => s.Id == Id);
            /// if the Drone wasnt found throw error
            if (drone is null)
            {
                throw new IdDosntExists("the Id could not be found", Id);
            }
            return drone;
        }

    
    }
} 
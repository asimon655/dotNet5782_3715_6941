using BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        /// <summary>
        /// get Binded Undelivered Parcel of a specific drone if not found return -1
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        int getBindedUndeliveredParcel(int droneId)
        {
            IEnumerable<DO.Parcel> parcels = data.GetParcels(x => x.DroneId == droneId && ParcelStatusC(x) != ParcelStatus.Delivered);
            DO.Parcel parcel = parcels.FirstOrDefault();
            if (parcel.DroneId != droneId)
            {
                return -1;
            }
            return parcel.Id;
        }
        /// <summary>
        /// calculate distance between two locations
        /// </summary>
        /// <param name="location1"></param>
        /// <param name="location2"></param>
        /// <returns></returns>
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
        /// <summary>
        /// return the id of closes station to a given location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        int getClosesStation(Location location)
        {
            int stationId = 0;
            double shortestDistance = double.MaxValue;
            foreach (var station in data.GetStations())
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
        /// <summary>
        /// get the power usage of a specific trip
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="weight">the weight of the loaded parcel</param>
        /// <returns></returns>
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
        /// <summary>
        /// return the location of the sender of a given parcel
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private Location getParcelLoctSender(DO.Parcel parcel)
        {
            try
            {
                DO.Customer CSMtmp = data.GetCustomer(parcel.SenderId);
                return new Location(CSMtmp.Longitude, CSMtmp.Lattitude);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        /// <summary>
        /// return the location of the sender of a given parcel
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private Location getParcelLoctTarget(DO.Parcel parcel)
        {
            try
            {
                DO.Customer CSMtmp = data.GetCustomer(parcel.TargetId);
                return new Location(CSMtmp.Longitude, CSMtmp.Lattitude);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        /// <summary>
        /// return true if the drone can reach the location of the sender/target of a given parcel
        /// </summary>
        /// <param name="drony"></param>
        /// <param name="parcel"></param>
        /// <param name="function">function to extract the location of the sender/target</param>
        /// <returns></returns>
        private bool canreach(DroneList drony, DO.Parcel parcel, Func<DO.Parcel, Location> function)
        {
            return getPowerUsage(drony.Loct, function(parcel), (WeightCategories)parcel.Weight) <= drony.Battery;
        }
        /// <summary>
        /// get drone from our list
        /// if not found throw IdDosntExists error
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private DroneList GetDroneToList(int Id)
        {
            DroneList drone = drones.FirstOrDefault(s => s.Id == Id);
            /// if the Drone wasnt found throw error
            if (drone.Id != Id)
            {
                throw new IdDosntExists("the Id could not be found", Id);
            }
            return drone;
        }
        /// <summary>
        /// generic function that check if a given value is in the range of his enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="EnumOutOfRange"></exception>
        void IsInEnum<T>(T value) where T : IConvertible
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new EnumOutOfRange("value not defined in the enum " + typeof(T), System.Convert.ToInt32(value));
            }
        }

    }
}
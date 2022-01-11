using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using Itinero.LocalGeo;

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
        internal static double calculateDistance(Location location1, Location location2)
        {
            Coordinate coordinate1 = new Coordinate(location1.Lattitude, location1.Longitude);
            Coordinate coordinate2 = new Coordinate(location2.Lattitude, location2.Longitude);

            return Coordinate.DistanceEstimateInMeter(coordinate1, coordinate2) / 1000;
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
        internal static double getPowerUsage(Location from, Location to, WeightCategories? weight = null)
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
        /// get the power usage of a specific trip
        /// </summary>
        /// <param name="travel"></param>
        /// <param name="weight">the weight of the loaded parcel</param>
        /// <returns></returns>
        internal static double getPowerUsage(double travel, WeightCategories? weight = null)
        {
            switch (weight)
            {
                case WeightCategories.Easy:
                    return travel * PowerConsumptionLight;
                case WeightCategories.Medium:
                    return travel * PowerConsumptionMedium;
                case WeightCategories.Heavy:
                    return travel * PowerConsumptionHeavy;
                default: // the drone is free
                    return travel * PowerConsumptionFree;
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
        static bool CanReach(DroneList drony, DO.Parcel parcel, Func<DO.Parcel, Location> function)
        {
            return getPowerUsage(drony.Loct, function(parcel), (WeightCategories)parcel.Weight) <= drony.Battery;
        }
        /// <summary>
        /// return true if the drone can pickup deliver and get to the nearest station
        /// </summary>
        /// <param name="drony"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        bool CanReach(DroneList drony, DO.Parcel parcel)
        {
            Location senderLoct = getParcelLoctSender(parcel);
            Location targetLoct = getParcelLoctTarget(parcel);
            DO.Station station = data.GetStation(getClosesStation(targetLoct));
            Location stationLoct = new Location(station.Longitude, station.Lattitude);

            double travel = getPowerUsage(drony.Loct, senderLoct, (WeightCategories)parcel.Weight) +
                            getPowerUsage(senderLoct, targetLoct, (WeightCategories)parcel.Weight) +
                            getPowerUsage(targetLoct, stationLoct);
            return travel <= drony.Battery;
        }
        /// <summary>
        /// get drone from our list
        /// if not found throw IdDosntExists error
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DroneList GetDroneToList(int Id)
        {
            DroneList drone = drones.FirstOrDefault(s => s.Id == Id);
            /// if the Drone wasnt found throw error
            if (!(drone is null ) && drone.Id != Id)
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
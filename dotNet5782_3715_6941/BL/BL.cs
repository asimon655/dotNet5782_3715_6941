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
    } 
}

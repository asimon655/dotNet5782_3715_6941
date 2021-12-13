using System;
using System.Collections.Generic;
using DO;

namespace DalApi
{
    public interface IDal
    {
        // CRUD of customer
        void AddCostumer(Costumer costumer);
        Costumer PullDataCostumer(int id);
        IEnumerable<Costumer> CostumersPrint();
        void UpdateCostumers(Costumer costumer);
        IEnumerable<Costumer> GetCostumers(Predicate<Costumer> expr);

        // CRUD of drone
        void AddDrone(Drone drone);
        Drone PullDataDrone(int id);
        IEnumerable<Drone> DronesPrint();
        void UpdateDrones(Drone drone);
        IEnumerable<Drone> GetDrones(Predicate<Drone> expr);

        // CRUD of station
        void AddStation(Station station);
        Station PullDataStation(int id);
        IEnumerable<Station> StationsPrint();
        void UpdateStations(Station station);
        IEnumerable<Station> GetStations(Predicate<Station> expr);

        // CRUD of parcel
        void AddParcel(Parcel parcel);
        Parcel PullDataParcel(int id);
        IEnumerable<Parcel> ParcelsPrint();
        void UpdateParcles(Parcel parcel);
        IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr);
        int CountParcels(Func<Parcel, bool> expr);

        // CRUD of dronecharge
        void AddDroneCharge(DroneCharge droneCharge);
        DroneCharge PullDataDroneChargeByDroneId(int droneId);
        IEnumerable<DroneCharge> DronesChargesPrint();
        IEnumerable<DroneCharge> GetDronesCharges(Predicate<DroneCharge> expr);
        int CountDronesCharges(Func<DroneCharge, bool> expr);
        void DeleteDroneCharge(int droneId);

        /// <summary>
        /// return the power usage consts
        /// </summary>
        /// <returns></returns>
        double[] GetPowerConsumption();
    }
}
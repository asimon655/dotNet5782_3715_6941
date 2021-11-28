using System;
using System.Collections;
using System.Collections.Generic;
using IDAL.DO;

namespace IDAL
{
    public interface Idal
    {
        // CRUD of customer
        void AddCostumer(Costumer costumer);
        Costumer PullDataCostumer(int id);
        IEnumerable<Costumer> CostumersPrint();
        void UpdateCostumers(Costumer costumer);

        // CRUD of drone
        void AddDrone(Drone drone);
        Drone PullDataDrone(int id);
        IEnumerable<Drone> DronesPrint();
        void UpdateDrones(Drone drone);

        // CRUD of station
        void AddStation(Station station);
        Station PullDataStation(int id);
        IEnumerable<Station> StationsPrint();
        void UpdateStations(Station station);
        /// <summary>
        /// get station with free charging slots
        /// </summary>
        /// <returns></returns>
        IEnumerable<Station> StaionsFreePortsPrint();

        // CRUD of parcel
        void AddParcel(Parcel parcel);
        Parcel PullDataParcel(int id);
        IEnumerable<Parcel> ParcelsPrint();
        void UpdateParcles(Parcel parcel);
        /// <summary>
        /// return list of unbinded parceles
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> ParcelWithoutDronePrint();

        // CRUD of dronecharge
        void AddDroneCharge(DroneCharge droneCharge);
        DroneCharge PullDataDroneChargeByDroneId(int droneId);
        IEnumerable<DroneCharge> DronesChargesPrint();
        void DeleteDroneCharge(int droneId);

        /// <summary>
        /// return the power usage consts
        /// </summary>
        /// <returns></returns>
        double[] GetPowerConsumption();
    }
}
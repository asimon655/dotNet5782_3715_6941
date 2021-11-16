using System;
using System.Collections;
using System.Collections.Generic;
using IDAL.DO;

namespace IDAL
{
    public interface Idal
    {
        void AddCostumer(Costumer costumer);
        Costumer PullDataCostumer(int id);
        IEnumerable<Costumer> CostumersPrint();
        void UpdateCostumers(Costumer costumer);

        void AddDrone(Drone drone);
        Drone PullDataDrone(int id);
        IEnumerable<Drone> DronesPrint();
        void UpdateDrones(Drone drone);

        void AddStation(Station station);
        Station PullDataStation(int id);
        IEnumerable<Station> StationsPrint();
        void UpdateStations(Station station);

        void AddParcel(Parcel parcel);
        Parcel PullDataParcel(int id);
        IEnumerable<Parcel> ParcelsPrint();
        void UpdateParcles(Parcel parcel);
        void BindDroneAndStaion(int DroneId, int staionId);
        double[] GetPowerConsumption();
    }
}
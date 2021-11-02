using System;
using System.Collections;

using IDAL.DO;

namespace IDAL
{
    public interface Idal
    {
        void AddCostumer(Costumer costumer);
        Costumer? PullDataCostumer(int id);
        IEnumerable<Costumer> CostumersPrint();
        void UpdateCostumers(Costumer costumer);

        void AddDrone(Drone drone);
        Drone? PullDataDrone(int id);
        IEnumerable<Drone> DronesPrint();
        void UpdateDrones(Drone drone);

        void AddStaion(Station station);
        Station? PullDataStation(int id);
        IEnumerable<Station> StaionsPrint();
        void UpdateStaions(Station station);

        void AddParcel(Parcel parcel);
        Parcel? PullDataParcel(int id);
        IEnumerable<Parcel> ParcelsPrint();
        void UpdateParcles(Parcel parcel);

        String DecimalToSexagesimal(double Longitude, double Latitude);
    }
}
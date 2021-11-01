using System;

using IDAL.DO;

namespace IDAL
{
    public interface Idal
    {
        void AddCostumer(Costumer costumer);
        Costumer? PullDataCostumer(int id);
        IEnumrable<Costumer> CostumersPrint();

        void AddDrone(Drone drone);
        Drone? PullDataDrone(int id);
        IEnumrable<Drone> DronesPrint();

        void AddStaion(Station station);
        Station? PullDataStation(int id);
        IEnumrable<Station> StaionsPrint();

        void AddParcel(Parcel parcel);
        Parcel? PullDataParcel(int id);
        IEnumrable<Parcel> ParcelsPrint();

        String DecimalToSexagesimal(double Longitude, double Latitude);
    }
}
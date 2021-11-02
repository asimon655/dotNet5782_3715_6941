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

        void AddDrone(Drone drone);
        Drone? PullDataDrone(int id);
        IEnumerable<Drone> DronesPrint();

        void AddStaion(Station station);
        Station? PullDataStation(int id);
        IEnumerable<Station> StaionsPrint();

        void AddParcel(Parcel parcel);
        Parcel? PullDataParcel(int id);
        IEnumerable<Parcel> ParcelsPrint();

        String DecimalToSexagesimal(double Longitude, double Latitude);
    }
}
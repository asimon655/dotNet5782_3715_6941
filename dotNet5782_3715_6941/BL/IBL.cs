using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface Ibl
    {
        /// add section
        void AddCostumer(BO.Costumer costumer);
        void AddDrone(BO.Drone drone, int stationId);
        void AddParcel(BO.Parcel parcel);
        void AddStation(BO.BaseStation station);

        /// get section
        BO.BaseStation PullDataStaion(int id);
        BO.Costumer PullDataCostumer(int id);
        BO.Drone PullDataDrone(int id);
        BO.Parcel PullDataParcel(int id);

        /// update section
        void UpdateDrone(int droneId, string droneName);
        void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null);
        void UpdateCostumer(int costumerId, string ? costumerName = null, string ? costumerPhone = null);
        void BindParcelToDrone(int droneId);
        void PickUpByDrone(int droneId);
        void ParcelDeliveredToCostumer(int droneId);
        void DroneCharge(int droneId);
        void DroneChargeRelease(int droneId, double chargingPeriod);

        /// list show section
        IEnumerable<BO.BaseStaionToList> StaionsPrint();
        IEnumerable<BO.BaseStaionToList> BaseStaionsFreePortsPrint();
        IEnumerable<BO.ClientToList> CostumersPrint();
        IEnumerable<BO.DroneToList> DronesPrint();
        IEnumerable<BO.ParcelToList> ParcelsPrint();
        IEnumerable<BO.ParcelToList> ParcelsWithoutDronesPrint();
        IEnumerable<BO.DroneToList> DronesPrintFiltered(Predicate<BO.DroneToList> drone);
    }
}

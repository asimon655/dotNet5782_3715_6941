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
        void AddCostumer(BO.Costumer);
        void AddDrone(BO.Drone);
        void AddParcel(BO.Parcel);
        void AddStaion(BO.Station);

        /// get section
        IBL.BO.Station PullDataStaion(int id);
        IBL.BO.Costumer PullDataCostumer(int id);
        IBL.BO.Drone PullDataDrone(int id);
        IBL.BO.Parcel PullDataParcel(int id);

        /// update section
        void UpdateDrone(int droneId, IBL.BO.Drone drone);
        void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null);
        void UpdateCostumer(int costumerId, int? costumerName = null, string? costumerPhone = null);
        void BindParcelToDrone(int droneId);
        void PickUpByDrone(int droneId);
        void ParcelDeliveredToCostumer(int droneId);
        void DroneCharge(int droneId);
        void DroneChargeRelease(int droneId, DateTime chargingPeriod);

        /// list show section
        IEnumerable<IBL.BO.Station> StaionsPrint();
        IEnumerable<IBL.BO.Station> BaseStaionsFreePortsPrint();
        IEnumerable<IBL.BO.Costumer> CostumersPrint();
        IEnumerable<IBL.BO.Drone> DronesPrint();
        IEnumerable<IBL.BO.Parcel> ParcelsPrint();
        IEnumerable<IBL.BO.Parcel> ParcelsWithoutDronesPrint();
    }
}

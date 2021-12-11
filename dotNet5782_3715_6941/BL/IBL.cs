using System.Collections.Generic;

namespace BlApi
{
    public interface Ibl
    {
        /// add section


        /// <summary>
        /// add new customer
        /// </summary>
        /// <param name="costumer"></param>
        void AddCostumer(BO.Costumer costumer);
        /// <summary>
        /// add new drone
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationId">id of the station to initialy put the drone in</param>
        void AddDrone(BO.Drone drone, int stationId);
        /// <summary>
        /// add a new parcel
        /// </summary>
        /// <param name="parcel"></param>
        void AddParcel(BO.Parcel parcel);
        /// <summary>
        /// add a new station
        /// </summary>
        /// <param name="station"></param>
        void AddStation(BO.BaseStation station);

        /// get section


        /// <summary>
        /// get station by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.BaseStation</returns>
        BO.BaseStation PullDataStaion(int id);
        /// <summary>
        /// get customerr by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Costumer</returns>
        BO.Costumer PullDataCostumer(int id);
        /// <summary>
        /// get drone by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Drone</returns>
        BO.Drone PullDataDrone(int id);
        /// <summary>
        /// get parcel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Parcel</returns>
        BO.Parcel PullDataParcel(int id);
        
        /// update section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="droneName"></param>
        void UpdateDrone(int droneId, string droneName);
        /// <summary>
        /// update the name or number of chagring slots of a specific station 
        /// </summary>
        /// <param name="stationId">the id of the station</param>
        /// <param name="stationName">optional</param>
        /// <param name="stationChargeSlots">optional</param>
        void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null);
        /// <summary>
        /// update the name or phone number of a specific customer
        /// </summary>
        /// <param name="costumerId">the id of the customer</param>
        /// <param name="costumerName">optional</param>
        /// <param name="costumerPhone">optional</param>
        void UpdateCostumer(int costumerId, string? costumerName = null, string? costumerPhone = null);
        /// <summary>
        /// order a drone to own a parcel
        /// the function chooses the best parcel by few paramerts
        /// </summary>
        /// <param name="droneId">the id of the drone</param>
        void BindParcelToDrone(int droneId);
        /// <summary>
        /// order a drone to pick up the parcel he owns from the sender
        /// </summary>
        /// <param name="droneId"></param>
        void PickUpByDrone(int droneId);
        /// <summary>
        /// order a drone to deliver the parcel he owns to the target
        /// </summary>
        /// <param name="droneId">the id of the drone</param>
        void ParcelDeliveredToCostumer(int droneId);
        /// <summary>
        /// order a drone to go to the nearest station to matance
        /// </summary>
        /// <param name="droneId">the id of the drone</param>
        void DroneCharge(int droneId);
        /// <summary>
        /// release a drone from matance
        /// and update the battery percenage
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargingPeriod"></param>
        void DroneChargeRelease(int droneId, double chargingPeriod);

        /// list show section

        /// <summary>
        /// returns the list of stations
        /// </summary>
        /// <returns>IEnumerable of BaseStaionToList</returns>
        IEnumerable<BO.BaseStaionToList> StaionsPrint();
        /// <summary>
        /// the function filter the stations and return thos with free charging slots
        /// </summary>
        /// <returns>IEnumerable of BaseStaionToList</returns>
        IEnumerable<BO.BaseStaionToList> BaseStaionsFreePortsPrint();
        /// <summary>
        /// returns the list of customers
        /// </summary>
        /// <returns>IEnumerable of ClientToList</returns>
        IEnumerable<BO.ClientToList> CostumersPrint();
        /// <summary>
        /// returns the list of drones
        /// </summary>
        /// <returns>IEnumerable of DroneToList</returns>
        IEnumerable<BO.DroneToList> DronesPrint();
        /// <summary>
        /// returns the list of parcels
        /// </summary>
        /// <returns>IEnumerable of ParcelToList</returns>
        IEnumerable<BO.ParcelToList> ParcelsPrint();
        /// <summary>
        /// the function filter the parcels and return thos that hasnt been binded to a drone
        /// </summary>
        /// <returns>IEnumerable of ParcelToList</returns>
        IEnumerable<BO.ParcelToList> ParcelsWithoutDronesPrint();
        IEnumerable<BO.DroneToList> DronesPrintFiltered(IEnumerable<BO.DroneStatuses> statuses, IEnumerable<BO.WeightCategories> weights);
    }
}

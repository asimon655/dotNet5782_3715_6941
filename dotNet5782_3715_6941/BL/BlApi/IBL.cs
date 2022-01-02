using System.Collections.Generic;

namespace BlApi
{
    public interface Ibl
    {
        #region Add
        /// <summary>
        /// add new customer
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(BO.Customer customer);
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
        void AddStation(BO.Station station);
        #endregion

        #region Get
        /// <summary>
        /// get station by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Station</returns>
        BO.Station GetStation(int id);
        /// <summary>
        /// get customerr by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Costumer</returns>
        BO.Customer GetCostumer(int id);
        /// <summary>
        /// get drone by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Drone</returns>
        BO.Drone GetDrone(int id);
        /// <summary>
        /// get parcel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BO.Parcel</returns>
        BO.Parcel GetParcel(int id);
        #endregion

        #region Update
        /// <summary>
        /// update drone name
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
        void DronePickUp(int droneId);
        /// <summary>
        /// order a drone to deliver the parcel he owns to the target
        /// </summary>
        /// <param name="droneId">the id of the drone</param>
        void DroneDelivere(int droneId);
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
        void DroneReleaseCharge(int droneId, double chargingPeriod);
        #endregion

        #region Get List
        /// <summary>
        /// returns the list of stations
        /// </summary>
        /// <returns>IEnumerable of StationList</returns>
        IEnumerable<BO.StationList> GetStations();
        /// <summary>
        /// the function filter the stations and return thos with free charging slots
        /// </summary>
        /// <returns>IEnumerable of StationList</returns>
        IEnumerable<BO.StationList> GetStationsWithFreePorts();
        /// <summary>
        /// returns the list of customers
        /// </summary>
        /// <returns>IEnumerable of CustomerList</returns>
        IEnumerable<BO.CustomerList> GetCustomers();
        /// <summary>
        /// returns the list of drones
        /// </summary>
        /// <returns>IEnumerable of DroneList</returns>
        IEnumerable<BO.DroneList> GetDrones();
        /// <summary>
        /// returns the list of parcels
        /// </summary>
        /// <returns>IEnumerable of ParcelList</returns>
        IEnumerable<BO.ParcelList> GetParcels();
        /// <summary>
        /// the function filter the parcels and return thos that hasnt been binded to a drone
        /// </summary>
        /// <returns>IEnumerable of ParcelList</returns>
        IEnumerable<BO.ParcelList> GetUnbindedParcels();
        /// <summary>
        /// return drones who their status and weight is in the parameters
        /// </summary>
        /// <param name="statuses"></param>
        /// <param name="weights"></param>
        /// <returns>IEnumerable of </returns>
        IEnumerable<BO.DroneList> GetDronesFiltered(IEnumerable<BO.DroneStatuses> statuses, IEnumerable<BO.WeightCategories> weights);
        #endregion
    
        #region Get Stats
        BO.DronesModelsStats GetDronesModelsStats();
        double[] GetDronesWeightsStats();
        double[] GetDronesStatusesStats();
        double[] GetParcelsPrioretiesStats();
        double[] GetParcelsStatusesStats();
        double[] GetParcelsWeightsStats();
        #endregion

        #region metadata
        /// <summary>
        /// download random pic from thispersondoesnotexist.com
        /// </summary>
        /// <returns></returns>
        string GetRandomPersonPic();
        /// <summary>
        /// return Drone pic by Model name
        /// if there is no saved pic it will download the first img from google photos search
        /// </summary>
        /// <param name="Model">model name of the drone</param>
        /// <returns>path of the pic</returns>
        string GetDronePic(string Model);
        /// <summary>
        /// return Customer pic by csutomer Id
        /// if there is no throw noPic
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>path of the pic</returns>
        string GetCustomerPic(int customerId);
        /// <summary>
        /// add pic to customer
        /// </summary>
        /// <param name="customerId">id of the customer</param>
        /// <param name="filepath">path to the image</param>
        void AddCustomerPic(int customerId, string filepath);
        /// <summary>
        /// return a capcha question and hashed answers
        /// the first object is the question and then comes the answers
        /// </summary>
        /// <returns>List Of string</returns>
        List<string> GetCapchaQuestion();
        #endregion
    }
}

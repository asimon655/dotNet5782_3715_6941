﻿using System.Collections.Generic;

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
        #region NEEDTOIMPLEMENTNOWSIMON
        //double[]  GetStationBusyPortsStats();
        // need to implement struct like    BO.DronesModelsStats with array of the vals of busy ports and another array of how many ports there are ( plz returt double and not int cause the drawing of 
        /// the graph uses double) 
        /// 
        //double[]  GetStationFreePortsStats();
        // need to implement struct like    BO.DronesModelsStats with array of the vals of busy ports and another array of how many ports there are ( plz returt double and not int cause the drawing of 
        /// the graph uses double) 
        /// 
        //double[]  GetCostumerReached();
        // need to implement struct like    BO.DronesModelsStats with array of the vals of busy ports and another array of how many ports there are ( plz returt double and not int cause the drawing of 
        /// the graph uses double) 
        /// 
        //double[]  GetCostumerUnReached();
        // need to implement struct like    BO.DronesModelsStats with array of the vals of busy ports and another array of how many ports there are ( plz returt double and not int cause the drawing of 
        /// the graph uses double) 
        /// 
        //double[]  GetCostumerParcelGot();
        // need to implement struct like    BO.DronesModelsStats with array of the vals of busy ports and another array of how many ports there are ( plz returt double and not int cause the drawing of 
        /// the graph uses double) 
        /// 
        //double[]  GetCostumerInTheWay();
        // need to implement struct like    BO.DronesModelsStats with array of the vals of busy ports and another array of how many ports there are ( plz returt double and not int cause the drawing of 
        /// the graph uses double) 
        /// 
        /// IEnumrable<BO.Costumer> SmartSearchCostumer(string ) ; 
        /// id or Name    
        /// IEnumrable<BO.Parcel> SmartSearchParcel(string ) ; 
        /// id or   Name
        /// IEnumrable<BO.Station> SmartSearchStation(string ) ;
        /// id or name - both numbers try id if the id doesnt exsist(Exception IdDoesntExsist) try name if it not both raise Exception
        /// IEnumrable<BO.Drone> SmartSearchDrone(string ) ; 
        /// id or Model 
        /// 
        ///string MostPoplurDroneModel () ; 
        /// returns the most popular model of the drone s
        ///BO.Drone DroneWithTheMostBattery(); 
        ///returns one of the drones with the biggest battery precentage - if there a few return one of the idk which one(first/last / whatever) 
        ///int AvarngeDistBetweenDrones(); 
        ///return the avarnge Destnation between 2 drones 
        ///
        ///BO.Costumer CostunerWithTheMostParcelsInTheWay () ; 
        /// return one of the cosumers with the biggest num of parcels in the way
        ///BO.Costumer CostunerWithTheMostParcelsUnReached () ; 
        /// return one of the cosumers with the biggest num of parcels unreached
        ///BO.Costumer CostunerWithTheMostParcelsGot () ; 
        /// return one of the cosumers with the biggest num of parcels got 
        /// 
        ///BO.Station MostFreeStation () ; 
        /// returns the most Free station
        ///BO.Station MostBusytation () ; 
        /// returns the most Busy station
        /// 
        /// IEnumerable<BO.DroneList> GetParcelsFiltered(DateTime ?  CreationFrom,  DateTime ? CreationTo , DateTime ? BindFrom,  DateTime ? BindTo , DateTime ?  PickUpFrom,  DateTime ? PickUpTo , DateTime ?  DeliverFrom,  DateTime ? DeliverTo , IEnumerable<BO.WeightCategories> weights , IEnumerable<BO.Prioreties> Priorties);
        ///   IEnumerable<BO.DroneList> GetStationFiltered(IEnumerable<int> FreePorts, IEnumerable<int> BusyPorts);
        ///   IEnumerable<BO.DroneList> GetCostumersFiltered(IEnumerable<int> reached, IEnumerable<int> Unreched ,IEnumerable<int> ParcelGot, IEnumerable<int> InTheWay );

        #endregion
    }
}

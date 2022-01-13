using DO;
using System;
using System.Collections.Generic;

namespace DalApi
{
    public interface IDal
    {
        #region CRUD of customer
        void AddCustomer(Customer customer);
        Customer GetCustomer(int id);
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Customer> GetCustomers(Predicate<Customer> expr);
        void UpdateCustomer(Customer costumer);
        int CountCustomers(Func<Customer, bool> expr);
        void DeleteCustomer(int id);
        #endregion

        #region CRUD of drone
        void AddDrone(Drone drone);
        Drone GetDrone(int id);
        IEnumerable<Drone> GetDrones();
        IEnumerable<Drone> GetDrones(Predicate<Drone> expr);
        void UpdateDrones(Drone drone);
        int CountDrones(Func<Drone, bool> expr);
        void DeleteDrone(int id);
        #endregion

        #region CRUD of station
        void AddStation(Station station);
        Station GetStation(int id);
        IEnumerable<Station> GetStations();
        IEnumerable<Station> GetStations(Predicate<Station> expr);
        void UpdateStations(Station station);
        int CountStations(Func<Station, bool> expr);
        void DeleteStation(int id);
        #endregion

        #region CRUD of parcel
        void AddParcel(Parcel parcel);
        Parcel GetParcel(int id);
        IEnumerable<Parcel> GetParcels();
        IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr);
        void UpdateParcles(Parcel parcel);
        int CountParcels(Func<Parcel, bool> expr);
        void DeleteParcel(int id);
        #endregion

        #region CRUD of dronecharge
        void AddDroneCharge(DroneCharge droneCharge);
        DroneCharge GetDroneCharge(int droneId);
        IEnumerable<DroneCharge> GetDronesCharges();
        IEnumerable<DroneCharge> GetDronesCharges(Predicate<DroneCharge> expr);
        int CountDronesCharges(Func<DroneCharge, bool> expr);
        void DeleteDroneCharge(int droneId);
        #endregion

        /// <summary>
        /// return the power usage consts
        /// </summary>
        /// <returns>double[]</returns>
        double[] GetPowerConsumption();
        DronePic GetDronePic(string Model);
        CustomerPic GetCustomerPic(int customerId);
        void AddDronePic(DronePic pic);
        void AddCustomerPic(CustomerPic pic);
    }
}
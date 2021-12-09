﻿using System;
using System.Collections.Generic;
using DO;



/// <summary>
/// important note - i couldt use foreach to change the class because foreach is by value with struct(with class it is by refrence ) so i had to go all over the array with reg for and change in the spesific index is  a lit bit exsosting but i didnt have power to be (hebrew=>) hahmolog 
/// </summary>
namespace DalObject
{
    class DataSource
    {
        internal static Random RandomGen = new Random();

        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Costumer> Costumers = new List<Costumer>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DronesCharges = new List<DroneCharge>();
        //until here array var declartion 
        static internal class Config
        {
            static internal double PowerConsumptionFree = 0.003;
            static internal double PowerConsumptionLight = 0.006;
            static internal double PowerConsumptionMedium = 0.008;
            static internal double PowerConsumptionHeavy = 0.001;
            static internal double ChargingSpeed = 33; 
            static internal int IdCreation = 0;
            static internal void Initalize()
            {
                //all the data is realistic - phone number have 10 digits  and id have 9 digits  

                /// constant for loop limit - right programing rules 
                const int StationInit = 2;
                const int DroneInit = 5;
                const int ParcelInit = 10;
                const int CostumerInit = 10;
                    

                for (int i = 0; i < StationInit; i++)
                    Stations.Add (new Station() {
                        Id = RandomGen.Next(100000000, 999999999),
                        ChargeSlots = RandomGen.Next(0, 1000),
                        Name = i,
                        Lattitude = RandomGen.NextDouble() * 45,
                        Longitude = RandomGen.NextDouble() * 45
                    });
                for (int i = 0; i < CostumerInit; i++)
                    Costumers.Add( new Costumer() {
                        Id = RandomGen.Next(100000000, 999999999),
                        Name = "Lev Cliet No." + i.ToString(),
                        Phone = "0" + RandomGen.Next(50, 59).ToString() + "-" + RandomGen.Next(100, 999).ToString() + "-" + RandomGen.Next(1000, 9999).ToString(),
                        Lattitude = RandomGen.NextDouble() * 45,
                        Longitude = RandomGen.NextDouble() * 45
                    });
                for (int i = 0; i < DroneInit; i++)
                    Drones.Add( new Drone() {
                        Id = RandomGen.Next(100000000, 999999999),
                        Modle = "V-Coptr Falcon",
                        MaxWeigth = (WeightCategories)RandomGen.Next(0, 2)
                    });
                for (int i = 0; i < ParcelInit; i++)
                {
                    DateTime scheduledtmp = new DateTime(1995, 1, 1).AddSeconds(RandomGen.Next(0, 86400)).AddDays(RandomGen.Next((DateTime.Today - new DateTime(1995, 1, 1)).Days));
                    /// created random time between today today to 1955 1th in janury 12:00:00 AM 
                    Parcel parcel = new Parcel() {
                        Id = ++IdCreation,
                        Priority = (Priorities)RandomGen.Next(0, 2),
                        Weight = (WeightCategories)RandomGen.Next(0, 2),
                        Schedulded = scheduledtmp,
                        Requested = null,
                        PickedUp = null,
                        Delivered = null,
                        DroneId = null,
                        SenderId = Costumers[RandomGen.Next(Costumers.Count)].Id,
                        TargetId = Costumers[RandomGen.Next(Costumers.Count)].Id
                    };
                    switch (i%4)
                    {
                        case (0):
                        {
                            DateTime requested =scheduledtmp.AddSeconds(RandomGen.Next(0, 86400*365));/// 86400 is one day in secs 
                                                                                                    /// addeed time to the randomed time i created before 
                            parcel.Requested = requested;
                            parcel.DroneId = Drones[RandomGen.Next(Drones.Count)].Id;
                            DateTime pickedup = requested.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                    /// addeed time to the randomed added  time i created before 
                            parcel.PickedUp = pickedup;
                            DateTime delivered = pickedup.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                    /// addeed time to the randomed added  added time i created before 
                            parcel.Delivered = delivered;
                        }
                        break;
                        case (1):
                        {
                            DateTime requested =scheduledtmp.AddSeconds(RandomGen.Next(0, 86400*365));/// 86400 is one day in secs 
                                                                                                    /// addeed time to the randomed time i created before 
                            parcel.Requested = requested;
                            parcel.DroneId = Drones[RandomGen.Next(Drones.Count)].Id;
                            DateTime pickedup = requested.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                    /// addeed time to the randomed added  time i created before 
                            parcel.PickedUp = pickedup;
                        }
                        break;
                        case (2):
                        {
                            DateTime requested =scheduledtmp.AddSeconds(RandomGen.Next(0, 86400*365));/// 86400 is one day in secs 
                                                                                                    /// addeed time to the randomed time i created before 
                            parcel.Requested = requested;
                            parcel.DroneId = Drones[RandomGen.Next(Drones.Count)].Id;
                        }
                        break;
                        case (3):
                        // not even binded
                        break;
                    }
                        
                    Parcels.Add (parcel);
                } 
            }


        }
    }

    public partial class DalObject : DalApi.IDal
    {
        public DalObject() => DataSource.Config.Initalize();


        static public void Update<T>(List<T> listy, T updater)
        {

            int IdObj = (int)(typeof(T).GetProperty("Id").GetValue(updater, null));

            for (int i = 0; i < listy.Count; i++)
            {


                int IdLst = (int)(typeof(T).GetProperty("Id").GetValue(listy[i], null));
                if (IdObj == IdLst)
                {
                    listy[i] = updater;
                    break;
                }
            }

        }
        public double[] GetPowerConsumption() => new double[] {
                                    DataSource.Config.PowerConsumptionFree,
                                    DataSource.Config.PowerConsumptionLight,
                                    DataSource.Config.PowerConsumptionMedium,
                                    DataSource.Config.PowerConsumptionHeavy,
                                    DataSource.Config.ChargingSpeed };
    }
}

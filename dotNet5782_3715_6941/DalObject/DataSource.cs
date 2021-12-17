using System.Collections.Generic;
using System;
using DO;


namespace Dal
{

    class DataSource
    {
        internal static Random RandomGen = new Random();

        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Customer> Costumers = new List<Customer>();
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
                    Stations.Add(new Station()
                    {
                        Id = RandomGen.Next(100000000, 999999999),
                        ChargeSlots = RandomGen.Next(0, 1000),
                        Name = i,
                        Lattitude = RandomGen.NextDouble() * 3 + 29,
                        Longitude = RandomGen.NextDouble() * 1 + 34.4
                    }) ;
                for (int i = 0; i < CostumerInit; i++)
                    Costumers.Add( new Customer() {
                        Id = RandomGen.Next(100000000, 999999999),
                        Name = "Lev Cliet No." + i.ToString(),
                        Phone = "0" + RandomGen.Next(50, 59).ToString() + "-" + RandomGen.Next(100, 999).ToString() + "-" + RandomGen.Next(1000, 9999).ToString(),
                        Lattitude = RandomGen.NextDouble() * 3 + 29,
                        Longitude = RandomGen.NextDouble() * 1 + 34.4
                    });
                for (int i = 0; i < DroneInit; i++)
                    Drones.Add( new Drone() {
                        Id = RandomGen.Next(100000000, 999999999),
                        Modle = "Mavic " + RandomGen.Next(1,11),
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

}
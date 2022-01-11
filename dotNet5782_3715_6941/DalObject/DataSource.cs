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
        internal static List<DronePic> DronePics = new List<DronePic>();
        internal static List<CustomerPic> CustomerPics = new List<CustomerPic>();
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
                const int StationInit = 20;
                const int DroneInit = 50;
                const int ParcelInit = 10;
                const int CostumerInit = 100;

                string[] names = { "James", "James", "John", "Michael", "William", "David", "David",
                                    "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan"};

                List<int> dronesDelivery = new List<int>();

                for (int i = 0; i < StationInit; i++)
                    Stations.Add(new Station()
                    {
                        Id = RandomGen.Next(1000000, 9999999),
                        ChargeSlots = RandomGen.Next(0, 50),
                        Name = "grand station " + i,
                        Lattitude = randomLattitude(),
                        Longitude = randomLongitude()
                    });
                for (int i = 0; i < CostumerInit; i++)
                    Costumers.Add( new Customer() {
                        Id = RandomGen.Next(1000000, 9999999),
                        Name = names[RandomGen.Next(names.Length)],
                        Phone = "0" + RandomGen.Next(50, 59).ToString() + "-" + RandomGen.Next(100, 1000).ToString() + "-" + RandomGen.Next(1000, 10000).ToString(),
                        Lattitude = randomLattitude(),
                        Longitude = randomLongitude()
                    });
                for (int i = 0; i < DroneInit; i++)
                {
                    Drone drone = new Drone()
                    {
                        Id = RandomGen.Next(1000000, 9999999),
                        Modle = "Mavic " + RandomGen.Next(1, 6),
                        MaxWeigth = (WeightCategories)RandomGen.Next(0, 2+1)
                    };
                    switch (i % 3)
                    {
                        case 0:
                            //delivery
                            dronesDelivery.Add(drone.Id);
                            break;
                        case 1:
                            // matnance
                            break;
                        case 2:
                            // free
                            break;
                    }
                    Drones.Add(drone);
                }
                for (int i = 0; i < ParcelInit; i++)
                {
                    int random = RandomGen.Next(Costumers.Count); 

                    DateTime scheduledtmp = new DateTime(1995, 1, 1).AddSeconds(RandomGen.Next(0, 86400)).AddDays(RandomGen.Next((DateTime.Today - new DateTime(1995, 1, 1)).Days));
                    /// created random time between today today to 1955 1th in janury 12:00:00 AM 
                    Parcel parcel = new Parcel() {
                        Id = ++IdCreation,
                        Priority = (Priorities)RandomGen.Next(0, 2+1),
                        Weight = (WeightCategories)RandomGen.Next(0, 2+1),
                        Schedulded = scheduledtmp,
                        Requested = null,
                        PickedUp = null,
                        Delivered = null,
                        DroneId = null,
                        SenderId = Costumers[random].Id,
                        TargetId = Costumers[(random+2)%Costumers.Count].Id
                    };

                    if ((i%4 == 1 || i%4 == 2) && dronesDelivery.Count > 0) // the on deliver parcels
                    {
                        DateTime requested = scheduledtmp.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                     /// addeed time to the randomed time i created before 
                        parcel.Requested = requested;
                        parcel.DroneId = dronesDelivery[RandomGen.Next(dronesDelivery.Count)];
                        dronesDelivery.Remove((int)parcel.DroneId); // the drone is on delivery

                        if (i%4 != 2) // i%4 == 1
                        {
                            DateTime pickedup = requested.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                     /// addeed time to the randomed added  time i created before 
                            parcel.PickedUp = pickedup;
                        }
                    }
                    else if (i%4 == 0) // delivered parcels
                    {
                        DateTime requested = scheduledtmp.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
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
                        
                    Parcels.Add (parcel);
                } 
            }

            static internal double randomLattitude()
            {
                return 34.740364d + RandomGen.NextDouble() * (34.909191 - 34.740364);
            }
            static internal double randomLongitude()
            {
                return 31.991251d + RandomGen.NextDouble() * (32.099796 - 31.991251d);
            }
        }
    }

}
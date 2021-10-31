using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using IDAL.DO;
/*  
 ___ ____  _   _ __  __
|_ _|  _ \| | | |  \/  |
 | || | | | |_| | |\/| |
 | || |_| |  _  | |  | |
|___|____/|_| |_|_|  |_| ©


    _        _
   / \   ___(_)_ __ ___   ___  _ __
  / _ \ / __| | '_ ` _ \ / _ \| '_ \
 / ___ \\__ \ | | | | | | (_) | | | |
/_/   \_\___/_|_| |_| |_|\___/|_| |_|

 */
/// <summary>
/// important note - i couldt use foreach to change the class because foreach is by value with struct(with class it is by refrence ) so i had to go all over the array with reg for and change in the spesific index is  a lit bit exsosting but i didnt have power to be (hebrew=>) hahmolog 
/// </summary>
namespace DAL
{
    namespace DalObject
    {
        public static class EnumHelper
        {
            // this method returns the description of a specific enum value
            // (the description is filled in the Enums.cs file
            // Usage : DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Menu>(IDAL.DO.Menu.Add)
            public static string GetDescription<T>(this T enumValue)
                where T : struct, IConvertible
            {
                // checking if T is infact an enum
                if (!typeof(T).IsEnum)
                    return null;

                var description = enumValue.ToString();
                var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

                if (fieldInfo != null)
                {
                    var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (attrs != null && attrs.Length > 0)
                    {
                        description = ((DescriptionAttribute)attrs[0]).Description;
                    }
                }

                return description;
            }
            
        }

        class DataSource
        {
            internal static Random RandomGen = new Random();

            internal static List<Drone> Drones = new List<Drone>();
            internal static List<Costumer> Costumers = new List<Costumer>();
            internal static List<Station> Stations = new List<Station>();
            internal static List<Parcel> Parcels = new List<Parcel>();
            internal const  int ParcelMaxSize = 100; /// const object is always a static object 
            internal const int CostumerMaxSize = 100; /// const object is always a static object 
            internal const int DroneMaxSize = 10; /// const object is always a static object 
            internal const int StaiomMaxSize = 5; /// const object is always a static object 
            internal static List<DroneCharge> DronesCharges = new List<DroneCharge>();
            //until here array var declartion 
            internal class Config
            {
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
                        Stations.Add (new Station
                        {
                            Id = RandomGen.Next(100000000, 999999999),
                            ChargeSlots = RandomGen.Next(0, 1000),
                            Name = i,
                            Latitude = RandomGen.NextDouble() * 45,
                            Longitude = RandomGen.NextDouble() * 45,
                        });
                    for (int i = 0; i < CostumerInit; i++)
                        Costumers.Add( new Costumer
                        {
                            Id = RandomGen.Next(100000000, 999999999),
                            Name = "Lev Cliet No." + i.ToString(),
                            Phone = "0" + RandomGen.Next(50, 59).ToString() + "-" + RandomGen.Next(100, 999).ToString() + "-" + RandomGen.Next(1000, 9999).ToString(),
                            Lattitude = RandomGen.NextDouble() * 45,
                            Longitude = RandomGen.NextDouble() * 45
                        });
                    for (int i = 0; i < DroneInit; i++)
                        Drones.Add( new Drone
                        {
                            Battery = RandomGen.NextDouble() * 100,
                            Id = RandomGen.Next(100000000, 999999999),
                            Modle = " V-Coptr Falcon",
                            Status = (DroneStatuses)RandomGen.Next(0, 1),
                            MaxWeigth = (WeightCategories)RandomGen.Next(0, 2)
                        });
                    for (int i = 0; i < ParcelInit; i++)
                    {
                        DateTime scheduledtmp = new DateTime(1995, 1, 1).AddSeconds(RandomGen.Next(0, 86400)).AddDays(RandomGen.Next((DateTime.Today - new DateTime(1995, 1, 1)).Days));
                        /// created random time between today today to 1955 1th in janury 12:00:00 AM 
                        DateTime requested =scheduledtmp.AddSeconds(RandomGen.Next(0, 86400*365));/// 86400 is one day in secs 
                                                                                                  /// addeed time to the randomed time i created before 
                        DateTime delivered = requested.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                  /// addeed time to the randomed added  time i created before 
                        DateTime pickedup = delivered.AddSeconds(RandomGen.Next(0, 86400 * 365));/// 86400 is one day in secs 
                                                                                                 /// addeed time to the randomed added  added time i created before 
                        Parcels.Add ( new Parcel
                        {
                            Id = ++IdCreation,
                            Priority = (Priorities)RandomGen.Next(0, 2),
                            Weight = (WeightCategories)RandomGen.Next(0, 2),
                            DroneId = RandomGen.Next(100000000, 999999999)
                        ,
                            Delivered =delivered ,
                            PickedUp =pickedup,
                            Requested = requested,
                            Schedulded = scheduledtmp,
                            SenderId = RandomGen.Next(100000000, 999999999),
                            TargetId = RandomGen.Next(100000000, 999999999)
                        });
                    } 
                }


            }
        }

        public class DalObject
        {
            static private int? CountStationFreeSlots(int  id)
            {
                Station? tmp = PullDataStation(id);
                ///check if the staion exsist and get it's id . 
                if (tmp is null)
                    return null;

                int countOccupied = 0;
                foreach (DroneCharge item in DataSource.DronesCharges)
                    if (item.StaionId == id)
                        countOccupied++;
                    /// counts how many chatge slots that binded to that staion there are 
                return (((Station)tmp).ChargeSlots - countOccupied); ///return the staion max minus the number that there realy is what gives as the free ports number 
            }

            static public void DroneCharge(int DroneId, int StationId) ///code that auto find free staion - after we wrote it yair said it is not neccary 
            {
                int drone_i = DataSource.Drones.FindIndex(item => item.Id == DroneId);
                if (drone_i == -1)
                    throw new Exception("the Drone Id is invalid");

                switch (DAL.DalObject.DataSource.Drones[drone_i].Status)
                {
                    case DroneStatuses.Matance:
                        throw new Exception("Drone is already in charge ");
                        break;
                    case DroneStatuses.Delivery:
                        throw new Exception("Drone is bussy at the momment in dilvery ");
                        break;
                    case DroneStatuses.Free:
                        // checking if the station exsits and has free slots
                        int? freeCount = CountStationFreeSlots(StationId);
                        if (freeCount is null)
                            throw new Exception("the station dosnt exsits");
                        if (freeCount == 0)
                            throw new Exception("there arent any left charging slots");

                        Drone drone = DataSource.Drones[drone_i];
                        drone.Status = DroneStatuses.Matance;
                        drone.Battery = 100;
                        DataSource.Drones[drone_i] = drone;

                        DataSource.DronesCharges.Add(new DroneCharge {
                                                        DroneId= DAL.DalObject.DataSource.Drones[drone_i].Id,
                                                        StaionId= StationId
                                                     });
                        break;
                }
            }
            static public void DroneChargeRealse(int id) /// realse drone fromthe staion and change his status to free 
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Drones.Length; i++)
                {
                    if (DAL.DalObject.DataSource.Drones[i].Id == id)
                    {
                        switch (DAL.DalObject.DataSource.Drones[i].Status)
                        {
                            case DroneStatuses.Free:
                                Console.WriteLine("Drone is not in charge ");
                                break;
                            case DroneStatuses.Delivery:
                                Console.WriteLine("Drone is bussy at the momment in dilvery ");
                                break;
                            case DroneStatuses.Matance:
                                // delete the record of the current charge
                                foreach (DroneCharge item in DataSource.DronesCharges)
                                    if (item.DroneId == id)
                                        DataSource.DronesCharges.Remove(item);
                                // free the drone status
                                DAL.DalObject.DataSource.Drones[i].Status = DroneStatuses.Free;
                                break;
                        }
                    }
                }
            }
            static public void PickUpByDrone(int ParceLId)/// change the drone to delivery and changes the pickedup Datetime to now . 
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                {
                    if (DAL.DalObject.DataSource.Parcels[i].Id == ParceLId)
                    {
                        for (int j = 0; j < DAL.DalObject.DataSource.Drones.Length; j++)
                        {
                            if (DAL.DalObject.DataSource.Drones[j].Id == DAL.DalObject.DataSource.Parcels[i].DroneId)
                            {
                                DataSource.Drones[j].Status = DroneStatuses.Delivery;
                                DAL.DalObject.DataSource.Parcels[i].PickedUp = DateTime.Now;
                            }
                        }
                    }
                }
            }

            static public void ParcelDeliveredToCostumer(int ParcelId) /// change the drone status to free and teh delivery time to now 
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                {
                    if (DAL.DalObject.DataSource.Parcels[i].Id == ParcelId)
                    {
                        DAL.DalObject.DataSource.Parcels[i].Delivered = DateTime.Now;
                        for (int j = 0; i < DAL.DalObject.DataSource.Drones.Length; j++)
                        {
                            if (DAL.DalObject.DataSource.Drones[j].Id == DAL.DalObject.DataSource.Parcels[i].Id)
                            {
                                DAL.DalObject.DataSource.Drones[j].Status = DroneStatuses.Free;
                            }
                        }
                    }
                }
            }
            // constractor calls Initalize
            static DalObject()///intalize the arrays
            {
                DataSource.Config.Initalize();
            }
            /// <summary>
            ///  all the adds checks if the space and if there is add the item to the array and return scuceed and if not return failed 
            /// </summary>
      
            static public void AddDrone(Drone cloned)
            {
                Drone? exists = PullDataDrone(cloned.Id);

                if (exists is null)
                {
                    throw new Exception("the Id Drone is already taken");
                }

                DataSource.Drones.Add(cloned);
            }

            static public void AddParcel(Parcel cloned)
            {
                cloned.Id = ++DataSource.Config.IdCreation;
                DataSource.Parcels.Add(cloned);
            }

            static public void AddCostumer(Costumer cloned)
            {
                Costumer? exists = PullDataCostumer(cloned.Id);

                if (exists is null)
                {
                    throw new Exception("the Id Costumer is already taken");
                }

                DataSource.Costumers.Add(cloned);
            }

            static public void AddStaion(Station cloned)
            {
                Station? exists = PullDataStation(cloned.Id);

                if (exists is null)
                {
                    throw new Exception("the Id Costumer is already taken");
                }

                DataSource.Stations.Add(cloned);
            }

            /// <summary>
            /// pulldata get id and search for it in the arr and if it is not finding it it returns null
            /// by the way i canceled the null safty because it sucks and makes me hard life 
            /// </summary>

            static public Drone? PullDataDrone(int _id)
            {
                Drone drone = DataSource.Drones.Find(s => s.Id == _id);
                /// if the Drone wasnt found return null
                if (drone.Id != _id)
                    return null;
                return drone;
            }
            static public Parcel? PullDataParcel(int _id)
            {
                Parcel parcel = DataSource.Parcels.Find(s => s.Id == _id);
                /// if the Parcel wasnt found return null
                if (parcel.Id != _id)
                    return null;
                return parcel;
            }
            static public Costumer? PullDataCostumer(int _id)
            {
                Costumer costumer = DataSource.Costumers.Find(s => s.Id == _id);
                /// if the Costumer wasnt found return null
                if (costumer.Id != _id)
                    return null;
                return costumer;
            }
            static public Station? PullDataStation(int _id)
            {
                Station station = DataSource.Stations.Find(s => s.Id == _id);
                /// if the Station wasnt found return null
                if (station.Id != _id)
                    return null;
                return station;
            }

            static public List<Station> StaionsPrint()
            {
                return DAL.DalObject.DataSource.Stations;
            }
            static public List<Drone> DronesPrint()
            {
                return DAL.DalObject.DataSource.Drones;
            }
            static public List<Costumer> CostumersPrint()
            {
                return DAL.DalObject.DataSource.Costumers;
            }
            static public List<Parcel> ParcelsPrint()
            {
                return DAL.DalObject.DataSource.Parcels;
            }
            /// <summary>
            /// if we need to print the all teh array except the deafults the prints return the whole array else it create a list and puts what we need to print and prints it 
            /// </summary>
 
            static public List<Parcel> ParcelsWithoutDronesPrint()
            {
                return DataSource.Parcels.FindAll(item => item.DroneId == default(Parcel).DroneId);
            }

            static public List<Station> BaseStaionsFreePortsPrint()
            {
                return DataSource.Stations.FindAll(item => CountStationFreeSlots(item.Id) > 0);
            }


            static public String  BindParcelToDrone(int Droneid , int ParcelId, int CostumerIdT, int CostumerIdS)/// BIND parcel to drone and src and dst clients ( peer to peer ) 
            {
                bool dronefound = false; 
                IDAL.DO.Parcel? tmp = DAL.DalObject.DalObject.PullDataParcel(ParcelId);
                if (tmp.Equals(null))
                   return ("Parcel didn't found ");
                else
                {
                    Costumer? tmp3 = DAL.DalObject.DalObject.PullDataCostumer(CostumerIdT);
                    if (tmp3.Equals(null))
                       return ("Target didnt found");
                    else
                    {
                        tmp3 = DAL.DalObject.DalObject.PullDataCostumer(CostumerIdS);
                        if (tmp3.Equals(null))
                           return ("Sender  didnt found");
                        else
                        {
                            for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                            {
                                if (DAL.DalObject.DataSource.Parcels[i].Id == ParcelId)
                                {
                                 
                                    for (int j = 0; j < DAL.DalObject.DataSource.Drones.Length; j++)
                                        if (DAL.DalObject.DataSource.Drones[j].Status == DroneStatuses.Free && (int)DAL.DalObject.DataSource.Drones[j].MaxWeigth >= (int)DAL.DalObject.DataSource.Parcels[i].Weight && Droneid == DAL.DalObject.DataSource.Drones[j].Id)
                                        {
                                            DAL.DalObject.DataSource.Parcels[i].DroneId = DAL.DalObject.DataSource.Drones[j].Id;
                                            dronefound = true; 
                                        }

                                    if (dronefound)
                                    {
                                        DAL.DalObject.DataSource.Parcels[i].Schedulded = DateTime.Now;
                                        DAL.DalObject.DataSource.Parcels[i].TargetId = CostumerIdT;
                                        DAL.DalObject.DataSource.Parcels[i].SenderId = CostumerIdS;
                                        return ("Object of Parcel binded to Object of Drone succefuly ");
                                    }
                                    else
                                        return "Drone didnt found or the drone found and it is status was in matnce or alrdeay in delivery  or weight category didnt fit to parcel";
                                }
                            }
                        }
                    }
                }
                return " bind failed "; 
            }
            /*
             
             
             ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~
             
             
             
             
                                    /\  \         /\  \         /\__\         /\__\         /\  \
                                   /::\  \       /::\  \       /::|  |       /:/  /        /::\  \
                                  /:/\:\  \     /:/\:\  \     /:|:|  |      /:/  /        /:/\ \  \
                                 /::\~\:\__\   /:/  \:\  \   /:/|:|  |__   /:/  /  ___   _\:\~\ \  \
                                /:/\:\ \:|__| /:/__/ \:\__\ /:/ |:| /\__\ /:/__/  /\__\ /\ \:\ \ \__\
                                \:\~\:\/:/  / \:\  \ /:/  / \/__|:|/:/  / \:\  \ /:/  / \:\ \:\ \/__/
                                 \:\ \::/  /   \:\  /:/  /      |:/:/  /   \:\  /:/  /   \:\ \:\__\
                                  \:\/:/  /     \:\/:/  /       |::/  /     \:\/:/  /     \:\/:/  /
                                   \::/__/       \::/  /        /:/  /       \::/  /       \::/  /
     ~~                             \/__/         \/__/         \/__/         \/__/

             
             
               ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~  ~bonus~
             
             
             */

            static public String DecimalToSexagesimal(double Longitude, double Latitude) /// calacs it with the well known algorithem that we found olnline ( beacuse u didnt gave that to us ) 
            {
                String result = "";
                // Longitude
                bool direction = (0 > Longitude);
                Longitude = Math.Abs(Longitude);
                result += Math.Floor(Longitude).ToString() + '\xb0';
                Longitude -= Math.Floor(Longitude);
                Longitude *= 60;
                result += Math.Floor(Longitude).ToString() + '`';
                Longitude -= Math.Floor(Longitude);
                Longitude *= 60;
                result += Math.Round(Longitude, 4).ToString() + "``";
                result += (direction ? 'N' : 'S');

                result += ' '; 

                // Latitude
                direction = (0 > Latitude);
                Latitude = Math.Abs(Latitude);
                result += Math.Floor(Latitude).ToString() + '\xb0';
                Latitude -= Math.Floor(Latitude);
                Latitude *= 60;
                result += Math.Floor(Latitude).ToString() + '`';
                Latitude -= Math.Floor(Latitude);
                Latitude *= 60;
                result += Math.Round(Latitude, 4).ToString() + "``";
                result += (direction ? 'E' : 'W');

                return result;
            }
        }
    }
}

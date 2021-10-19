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
|___|____/|_| |_|_|  |_|


    _        _
   / \   ___(_)_ __ ___   ___  _ __
  / _ \ / __| | '_ ` _ \ / _ \| '_ \
 / ___ \\__ \ | | | | | | (_) | | | |
/_/   \_\___/_|_| |_| |_|\___/|_| |_|

 */
namespace DAL
{
    namespace DalObject
    {
        public static class EnumHelper
        {
            public static string GetDescription<T>(this T enumValue)
                where T : struct, IConvertible
            {
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

            internal static Drone[] Drones = new Drone[10];
            internal static Costumer[] Costumers = new Costumer[100];
            internal static Station[] Staions = new Station[5];
            internal static IDAL.DO.Parcel[] Parcels = new IDAL.DO.Parcel[1000];
            //until here arrady var declartion 
            internal class Config
            {

                static internal int DronesFirst = 0;
                static internal int CostumerFirst = 0;
                static internal int StaionsFirst = 0;
                static internal int ParcelFirst = 0;
                static internal int idcreation = 1;

                static internal void Initalize()
                {
                    //all the data is realistic - phone number have 10 digits  and id have 9 digits  
                    CostumerFirst = 10;
                    DronesFirst = 5;
                    StaionsFirst = 2;
                    ParcelFirst = 10;
                    for (int i = 0; i < 2; i++)
                        Staions[i] = new Station { Id = RandomGen.Next(100000000, 999999999), ChargeSlots = RandomGen.Next(0, 1000), Name = i, Latitude = RandomGen.NextDouble() * 45, Longitude = RandomGen.NextDouble() * 45 };
                    for (int i = 0; i < 10; i++)
                        Costumers[i] = new Costumer { Id = RandomGen.Next(100000000, 999999999), Name = "Lev Cliet No." + i.ToString(), Phone = "0" + RandomGen.Next(50, 59).ToString() + "-" + RandomGen.Next(100, 999).ToString() + "-" + RandomGen.Next(1000, 9999).ToString(), Lattitude = RandomGen.NextDouble() * 45, Longitude = RandomGen.NextDouble() * 45 };
                    for (int i = 0; i < 5; i++)
                        Drones[i] = new Drone { Battery = RandomGen.NextDouble() * 100, id = RandomGen.Next(100000000, 999999999), Modle = " V-Coptr Falcon", Status = (DroneStatuses)RandomGen.Next(0, 1), MaxWeigth = (WeightCategories)RandomGen.Next(0, 2) };
                    for (int i = 0; i < 10; i++)
                        Parcels[i] = new IDAL.DO.Parcel { Id = idcreation++, Priority = (Priorities)RandomGen.Next(0, 2), Weight = (WeightCategories)RandomGen.Next(0, 2) };
                }


            }
        }

        public class DalObject
        {
            static public void DroneCharge(int id)
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Drones.Length; i++)
                    if (DAL.DalObject.DataSource.Drones[i].id == id)
                    {
                        if (DAL.DalObject.DataSource.Drones[i].Status == DroneStatuses.Matance)
                            Console.WriteLine("Drone is already in charge ");
                        else
                        {
                            if (DAL.DalObject.DataSource.Drones[i].Status == DroneStatuses.Delivery)
                                Console.WriteLine("Drone is bussy at the momment in dilvery ");
                            else
                            {
                                DAL.DalObject.DataSource.Drones[i].Status = DroneStatuses.Matance;
                                DAL.DalObject.DataSource.Drones[i].Battery = 100;

                            }
                        }


                     }



            }
            static public void DroneChargeRealse(int id)
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Drones.Length; i++)
                    if (DAL.DalObject.DataSource.Drones[i].id == id)
                    {
                        if (DAL.DalObject.DataSource.Drones[i].Status == DroneStatuses.Free)
                            Console.WriteLine("Drone is not in charge ");
                        else
                        {
                            if (DAL.DalObject.DataSource.Drones[i].Status == DroneStatuses.Delivery)
                                Console.WriteLine("Drone is bussy at the momment in dilvery ");
                            else
                            {
                                DAL.DalObject.DataSource.Drones[i].Status = DroneStatuses.Free;
      

                            }
                        }


                    }



            }
            static public void PickUpByDrone(int ParceLId)
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                {


                    if (DAL.DalObject.DataSource.Parcels[i].Id == ParceLId)
                        for (int j = 0; j < DAL.DalObject.DataSource.Drones.Length; j++)
                            if (DAL.DalObject.DataSource.Drones[j].id == DAL.DalObject.DataSource.Parcels[i].DroneId)
                            {
                                DataSource.Drones[j].Status = DroneStatuses.Delivery;
                                DAL.DalObject.DataSource.Parcels[i].PickedUp = DateTime.Now;
                            }





                }


            }

            static public void ParcelDeliveredToCostumer(int ParcelId)
            {
                for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                    if (DAL.DalObject.DataSource.Parcels[i].Id == ParcelId)
                    {
                        DAL.DalObject.DataSource.Parcels[i].Delivered = DateTime.Now;
                        for (int j = 0; i < DAL.DalObject.DataSource.Drones.Length; j++)
                            if (DAL.DalObject.DataSource.Drones[j].id == DAL.DalObject.DataSource.Parcels[i].Id)
                            {
                                DAL.DalObject.DataSource.Drones[j].Status = DroneStatuses.Free;


                            }
                    }






            }

            static DalObject()
            {
                DataSource.Config.Initalize();
            }
            static public void AddDrone(Drone cloned)
            {

                cloned.id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Drones[DataSource.Config.DronesFirst++] = cloned;


            }
            static public void AddParcel(IDAL.DO.Parcel cloned)
            {
                DataSource.Parcels[DataSource.Config.ParcelFirst++] = cloned;

            }
            static public void AddCostumer(Costumer cloned)
            {
                cloned.Id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Costumers[DataSource.Config.CostumerFirst++] = cloned;

            }
            static public void AddStaion(Station cloned)
            {
                cloned.Id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Staions[DataSource.Config.StaionsFirst++] = cloned;


            }

            static public void UpdateData()
            {



            }
            static public Drone? PullDataDrone(int _id)
            {
                foreach (Drone item in DataSource.Drones)
                    if (item.id == _id)
                        return item;
                return null;
            }
            static public IDAL.DO.Parcel? PullDataParcel(int _id)
            {
                foreach (IDAL.DO.Parcel item in DataSource.Parcels)
                    if (item.Id == _id)
                        return item;
                return null;
            }
            static public Costumer? PullDataCostumer(int _id)
            {
                foreach (Costumer item in DataSource.Costumers)
                    if (item.Id == _id)
                        return item;
                return null;
            }
            static public Station? PullDataStaion(int _id)
            {
                foreach (Station item in DataSource.Staions)
                    if (item.Id == _id)
                        return item;
                return null;
            }
            static public void StaionsPrint()
            {
                foreach (Station item in DAL.DalObject.DataSource.Staions)
                {
                    if (!(item.Equals(default(Station))))
                        Console.WriteLine(item);
                }


            }
            static public void DronesPrint()
            {
                foreach (Drone item in DAL.DalObject.DataSource.Drones)
                {
                    if (!(item.Equals(default(Drone))))
                        Console.WriteLine(item);
                }


            }


            static public void ParcelsPrint()
            {
                foreach (IDAL.DO.Parcel item in DAL.DalObject.DataSource.Parcels)
                {
                    if (!(item.Equals(default(IDAL.DO.Parcel))))
                        Console.WriteLine(item);
                }


            }
            static public void ParcelsWithotDronesPrint()
            {
                foreach (IDAL.DO.Parcel item in DAL.DalObject.DataSource.Parcels)
                {
                    if (!(item.Equals(default(IDAL.DO.Parcel))) && item.DroneId == default(IDAL.DO.Parcel).Id)
                        Console.WriteLine(item);
                }


            }
            static public void CostumersPrint()
            {
                foreach (Costumer item in DAL.DalObject.DataSource.Costumers)
                {
                    if (!(item.Equals(default(Costumer))))
                        Console.WriteLine(item);
                }


            }
            static public void BindParcelToDrone(int ParcelId, int CostumerIdT, int CostumerIdS)
            {
                IDAL.DO.Parcel? tmp = DAL.DalObject.DalObject.PullDataParcel(ParcelId);
                if (tmp.Equals(null))
                    Console.WriteLine("Parcel didn't found ");
                else
                {


                    Costumer? tmp3 = DAL.DalObject.DalObject.PullDataCostumer(CostumerIdT);
                    if (tmp3.Equals(null))
                        Console.WriteLine("Target didnt found");
                    else
                    {
                        tmp3 = DAL.DalObject.DalObject.PullDataCostumer(CostumerIdS);
                        if (tmp3.Equals(null))
                            Console.WriteLine("Sender  didnt found");
                        else
                        {
                            for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                                if (DAL.DalObject.DataSource.Parcels[i].Id == ParcelId)
                                {
                                    DAL.DalObject.DataSource.Parcels[i].TargetId = CostumerIdT;
                                    DAL.DalObject.DataSource.Parcels[i].SenderId = CostumerIdS;
                                    for (int j = 0; j < DAL.DalObject.DataSource.Drones.Length; j++)
                                        if (DAL.DalObject.DataSource.Drones[j].Status == DroneStatuses.Free && (int)DAL.DalObject.DataSource.Drones[j].MaxWeigth >= (int)DAL.DalObject.DataSource.Parcels[i].Weight)

                                            DAL.DalObject.DataSource.Parcels[i].Id = DAL.DalObject.DataSource.Drones[j].id;




                                    DAL.DalObject.DataSource.Parcels[i].Schedulded = DateTime.Now;

                                    Console.WriteLine("Object of Parcel binded to Object of Drone succefuly ");
                                }

                        }
                    }


                }


            }

        }

    }
}

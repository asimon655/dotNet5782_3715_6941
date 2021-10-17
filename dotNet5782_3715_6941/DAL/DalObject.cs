using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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

            internal static IDAL.DO.Drone[] Drones = new IDAL.DO.Drone[10];
            internal static IDAL.DO.Costumer[] Costumers = new IDAL.DO.Costumer[100];
            internal static IDAL.DO.Staion[] Staions = new IDAL.DO.Staion[5];
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
                        Staions[i] = new IDAL.DO.Staion {Id= RandomGen.Next(100000000,999999999), ChargeSlots= RandomGen.Next(0,1000) , Name=i ,Latitude=RandomGen.NextDouble()*45 , Longitude=RandomGen.NextDouble()*45 };
                    for (int i = 0; i < 10; i++)
                        Costumers[i] = new IDAL.DO.Costumer { Id = RandomGen.Next(100000000, 999999999) ,Name="Lev Cliet No."+i.ToString(), Phone=( 1000000000 + (long)(RandomGen.NextDouble() * (9999999999-1000000000))).ToString() , Lattitude  = RandomGen.NextDouble() * 45, Longitude = RandomGen.NextDouble() * 45 };
                    for (int i = 0; i < 5; i++)
                        Drones[i] = new IDAL.DO.Drone {Battery=RandomGen.NextDouble()*100, id = RandomGen.Next(100000000, 999999999),Modle= " V-Coptr Falcon", Status=(IDAL.DO.DroneStatuses)RandomGen.Next(0,1),MaxWeigth=(IDAL.DO.WeightCategories)RandomGen.Next(0,2) };
                    for (int i = 0; i < 10; i++)
                        Parcels[i] = new IDAL.DO.Parcel {Id =idcreation++,Delivered=DateTime.Now, PickedUp=DateTime.Now, Priority=(IDAL.DO.Priorities)RandomGen.Next(0,2), Weight= (IDAL.DO.WeightCategories)RandomGen.Next(0,2)};
                }


            }
        }

        public class DalObject 
        {
             
           static    DalObject()
            {
                DataSource.Config.Initalize();
            }
            static public void AddDrone(IDAL.DO.Drone cloned) {

                cloned.id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Drones[DataSource.Config.DronesFirst++] = cloned;

            
            } 
            static public void AddParcel( IDAL.DO.Parcel cloned )
            {
                DataSource.Parcels[DataSource.Config.ParcelFirst++] = cloned; 

            }
            static public void AddCostumer(IDAL.DO.Costumer cloned)
            {
                cloned.Id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Costumers[DataSource.Config.CostumerFirst++] = cloned; 
            
            }
            static public void AddStaion(IDAL.DO.Staion cloned)
            {
                cloned.Id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Staions[DataSource.Config.StaionsFirst++] = cloned; 

            
            }

            static public void UpdateData()
            {
            

            
            }
            static public IDAL.DO.Drone? PullDataDrone (int _id )
            {
                foreach (IDAL.DO.Drone item in DataSource.Drones)
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
            static public IDAL.DO.Costumer? PullDataCostumer(int _id)
            {
                foreach (IDAL.DO.Costumer item in DataSource.Costumers)
                    if (item.Id == _id)
                        return item;
                return null;
            }
            static public IDAL.DO.Staion? PullDataStaion(int _id)
            {
                foreach (IDAL.DO.Staion item in DataSource.Staions)
                    if (item.Id == _id)
                        return item;
                return null;
            }
            static public void StaionsPrint()
            {
                foreach (IDAL.DO.Staion item in DAL.DalObject.DataSource.Staions)
                {
                    if (!(item  .Equals( default(IDAL.DO.Staion))))
                        Console.WriteLine(item);
                }
            
            
            }
            static public void DronesPrint()
            {
                foreach (IDAL.DO.Drone item in DAL.DalObject.DataSource.Drones)
                {
                    if (!(item.Equals(default(IDAL.DO.Drone))))
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
                    if (!(item.Equals(default(IDAL.DO.Parcel))) && item.DroneId== default(IDAL.DO.Parcel).Id)
                        Console.WriteLine(item);
                }


            }
            static public void CostumersPrint()
            {
                foreach (IDAL.DO.Costumer item in DAL.DalObject.DataSource.Costumers)
                {
                    if (!(item.Equals(default(IDAL.DO.Costumer))))
                        Console.WriteLine(item);
                }


            }
            static public void BindParcelToDrone(int ParcelId, int DroneId)
            {
                IDAL.DO.Parcel? tmp = DAL.DalObject.DalObject.PullDataParcel(ParcelId);
                if (tmp.Equals(null))
                    Console.WriteLine("Parcel didn't found ");
                else
                {
                    IDAL.DO.Drone? tmp2 = DAL.DalObject.DalObject.PullDataDrone(DroneId);
                    if (tmp2.Equals(null))
                        Console.WriteLine("Droen didn't found (and parcel))");
                    else
                    {
                        for (int i = 0; i < DAL.DalObject.DataSource.Parcels.Length; i++)
                            if (DAL.DalObject.DataSource.Parcels[i].Id == ParcelId)
                                DAL.DalObject.DataSource.Parcels[i].DroneId = DroneId;
                        Console.WriteLine("Object of Parcel binded to Object of Drone succefuly ");
                                

                    }
                
                
                }
            
            
            }

        }
        static public void PickUpByDrone(int ParceLId)
        {
            for (int i = 0; i < DAL.DalObject.DataSource.Drones.Length; i++)
            {
                if (DAL.DalObject.DataSource.Drones[i].Status == IDAL.DO.DroneStatuses.Free)
                {
                    DAL.DalObject.DataSource.Drones[i].Status = IDAL.DO.DroneStatuses.Delivery;
                    for (int J = 0; i < DAL.DalObject.DataSource.Parcels.Length; J++)
                    {
                        if (DAL.DalObject.DataSource.Parcels[J].Id == ParceLId)
                            DAL.DalObject.DataSource.Parcels[J].DroneId = DAL.DalObject.DataSource.Drones[i].id;
                        Console.WriteLine("Parcel: {1} picked up by drone {1}", DAL.DalObject.DataSource.Parcels[J].Id, DAL.DalObject.DataSource.Parcels[J].DroneId);
                    
                    } 
                }





            }
        
        
        } 
    }
}

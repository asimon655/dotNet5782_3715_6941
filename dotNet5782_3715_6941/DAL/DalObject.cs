using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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
            //until here array var declartion 
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
                        Staions[i] = new IDAL.DO.Staion {Id= RandomGen.Next(100000000,999999999), ChargeSlots= RandomGen.Next() , Name=i ,Latitude=RandomGen.NextDouble()*45 , Longitude=RandomGen.NextDouble()*45 };
                    for (int i = 0; i < 10; i++)
                        Costumers[i] = new IDAL.DO.Costumer { Id = RandomGen.Next(100000000, 999999999) ,Name="Lev Cliet No."+i.ToString(), Phone=( 1000000000 + (long)(RandomGen.NextDouble() * (9999999999-1000000000))).ToString() , Lattitude  = RandomGen.NextDouble() * 45, Longitude = RandomGen.NextDouble() * 45 };
                    for (int i = 0; i < 5; i++)
                        Drones[i] = new IDAL.DO.Drone {Battery=RandomGen.NextDouble()*100, id = RandomGen.Next(100000000, 999999999),Modle= " V-Coptr Falcon", Status=(IDAL.DO.DroneStatuses)RandomGen.Next(0,1),MaxWeigth=(IDAL.DO.WeightCategories)RandomGen.Next(0,2) };
                    for (int i = 0; i < 10; i++)
                        Parcels[i] = new IDAL.DO.Parcel {Id =idcreation++,Delivered=DateTime.Now, DroneId = RandomGen.Next(100000000, 999999999), PickedUp=DateTime.Now, Priority=(IDAL.DO.Priorities)RandomGen.Next(0,2), Weight= (IDAL.DO.WeightCategories)RandomGen.Next(0,2)};
                    
                  



                }


            }




        }
        public class DalObject 
        {
             
            public DalObject()
            {
                DataSource.Config.Initalize();
            }
            public void AddDroe(IDAL.DO.Drone cloned) {

                cloned.id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Drones[DataSource.Config.DronesFirst++] = cloned;

            
            } 
            public void AddParcel( IDAL.DO.Parcel cloned )
            {
                DataSource.Parcels[DataSource.Config.ParcelFirst++] = cloned; 

            }
            public void AddCostumer(IDAL.DO.Costumer cloned)
            {
                cloned.Id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Costumers[DataSource.Config.CostumerFirst++] = cloned; 
            
            }
            public void AddStaion(IDAL.DO.Staion cloned)
            {
                cloned.Id = DataSource.RandomGen.Next(100000000, 999999999);
                DataSource.Staions[DataSource.Config.StaionsFirst++] = cloned; 

            
            }

            public void UpdateData()
            {
            

            
            }
            public IDAL.DO.Drone? PullDataDrone (int _id )
            {
                foreach (IDAL.DO.Drone item in DataSource.Drones)
                    if (item.id == _id)
                        return item;
                return null;
            }

        
        
            
        
        }



    }
}

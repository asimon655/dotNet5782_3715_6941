using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
        class DataSource
        {
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
                static private Random RandomGen = new Random(); 
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
            public void AddDrone(int _id ,double _Battery , String _Model , IDAL.DO.WeightCategories _MaxWeight , IDAL.DO.DroneStatuses _droneStatuses)
            {

                DataSource.Drones[DataSource.Config.DronesFirst++] = new IDAL.DO.Drone { id = _id, Battery = _Battery, Modle = _Model, Status = _droneStatuses, MaxWeigth = _MaxWeight };
                
            }
            public void AddParcel(int _Senderid , int _Targetid,IDAL.DO.WeightCategories _Weight , IDAL.DO.Priorities _Priorty , DateTime _Requested , int _DroneId , DateTime _Scheduled , DateTime _PickedUp , DateTime _Delivered )
            {
                int _id = DataSource.Config.idcreation++;

                DataSource.Parcels[DataSource.Config.ParcelFirst++] = new IDAL.DO.Parcel { Id = _id, SenderId = _Senderid, TargetId = _Targetid, Weight = _Weight, Priority = _Priorty, Requested = _Requested, DroneId = _DroneId, Schedulded = _Scheduled, PickedUp = _PickedUp, Delivered = _Delivered };
            
            }
            public void UpdateData()
            {
            

            
            }
            public void PullData()
            { 
            }

        
        
            
        
        }



    }
}

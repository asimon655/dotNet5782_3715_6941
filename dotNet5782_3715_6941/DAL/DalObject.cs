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
                static internal int idcreation = 0;
                static internal void Initalize()
                {
                    CostumerFirst = 10;
                    DronesFirst = 5;
                    StaionsFirst = 2;
                    ParcelFirst = 10;
                    for (int i = 0; i < 2; i++)
                    {

                        Staions[i] = new IDAL.DO.Staion();
                    }
                    for (int i = 0; i < 10; i++)
                    {

                        Costumers[i] = new IDAL.DO.Costumer();
                        Costumers[i].
                    }
                    for (int i = 0; i < 5; i++)
                    {

                        Drones[i] = new IDAL.DO.Drone();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        Parcels[i] = new IDAL.DO.Parcel();
                    }





                }


            }




        }



    }
}

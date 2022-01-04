﻿using System.Collections.Generic;



/// <summary>
/// important note - i couldt use foreach to change the class because foreach is by value with struct(with class it is by refrence ) so i had to go all over the array with reg for and change in the spesific index is  a lit bit exsosting but i didnt have power to be (hebrew=>) hahmolog 
/// </summary>
namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        public DalObject() {
            DataSource.Config.Initalize();
        }

        static DalObject() {}
        internal static readonly object padlock = new object();
        internal static DalObject instance;
         static DalObject Instance {
            get {
                if (instance == null) {
                    lock(padlock) {  
                        if (instance == null) {  
                            instance = new DalObject();  
                        }
                    }
                }
                return instance;  
            }
        }

        static int Update<T>(List<T> listy, T updater)
        {
            var id = typeof(T).GetProperty("Id");
            var isDeleted = typeof(T).GetProperty("IsDeleted");

            int updaterId = (int)id.GetValue(updater, null);

            int index = listy.FindIndex(x => !(bool)isDeleted.GetValue(x, null) && (int)id.GetValue(x, null) == updaterId);

            if (index != -1)
                listy[index] = updater;

            return index;
        }
        static int Delete<T>(List<T> listy, int deleteId)
        {
            var id = typeof(T).GetProperty("Id");
            var isDeleted = typeof(T).GetProperty("IsDeleted");

            int index = listy.FindIndex(x => !(bool)isDeleted.GetValue(x, null) && (int)id.GetValue(x, null) == deleteId);

            if (index != -1) {
                T updater = listy[index];
                isDeleted.SetValue(updater, true);
                listy[index] = updater;
            }

            return index;
        }
        public double[] GetPowerConsumption() => new double[] {
                                    DataSource.Config.PowerConsumptionFree,
                                    DataSource.Config.PowerConsumptionLight,
                                    DataSource.Config.PowerConsumptionMedium,
                                    DataSource.Config.PowerConsumptionHeavy,
                                    DataSource.Config.ChargingSpeed };
    }
}

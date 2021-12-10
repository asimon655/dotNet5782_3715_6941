using System.Collections.Generic;



/// <summary>
/// important note - i couldt use foreach to change the class because foreach is by value with struct(with class it is by refrence ) so i had to go all over the array with reg for and change in the spesific index is  a lit bit exsosting but i didnt have power to be (hebrew=>) hahmolog 
/// </summary>
namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        public DalObject() {
            DataSource.Config.Initalize();
        }


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

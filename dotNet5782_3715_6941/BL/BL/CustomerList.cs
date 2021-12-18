using System.Collections.Generic;
using BO;


namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {

        public IEnumerable<CustomerList> GetCustomers()
        {
            List<CustomerList> tmpy = new List<CustomerList>();
            foreach(DO.Customer x in data.GetCustomers())
            {
                tmpy.Add(CltToLstC(x));
            }
            return tmpy;
        }
        

    }
} 
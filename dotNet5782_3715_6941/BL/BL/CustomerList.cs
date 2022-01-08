using System.Collections.Generic;
using System.Linq;
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
                tmpy.Add(ConvertList(x));
            }
            return tmpy;
        }

        public IEnumerable<BO.CustomerList> GetCostumersFiltered(IEnumerable<int>? reached, IEnumerable<int>? Unreched, IEnumerable<int>? ParcelGot, IEnumerable<int>? InTheWay)
        {
            return data.GetCustomers(x => (reached is null || reached.Contains(data.CountParcels(x => x.SenderId == x.Id && ParcelStatusC(x) == ParcelStatus.Delivered))) &&
                                    (Unreched is null || Unreched.Contains(data.CountParcels(x => x.SenderId == x.Id && ParcelStatusC(x) != ParcelStatus.Delivered))) &&
                                    (ParcelGot is null || ParcelGot.Contains(data.CountParcels(x => x.TargetId == x.Id && ParcelStatusC(x) == ParcelStatus.Delivered))) &&
                                    (InTheWay is null || InTheWay.Contains(data.CountParcels(x => x.TargetId == x.Id && ParcelStatusC(x) != ParcelStatus.Delivered))))
                    .Select(ConvertList);
        }
    }
} 
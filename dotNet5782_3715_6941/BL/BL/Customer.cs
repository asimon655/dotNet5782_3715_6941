using BO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCostumer(int id)
        {
            try
            {
                return Convert(data.GetCustomer(id));
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer costumer)
        {
            if (costumer.Loct.Lattitude > 90 || costumer.Loct.Lattitude < -90 || costumer.Loct.Longitude > 180 || costumer.Loct.Longitude < -180)
            {
                throw new LocationOutOfRange("the Location Values are out of boundries  :  ", costumer.Loct.Longitude, costumer.Loct.Lattitude);
            }

            DO.Customer CostumerTmp = new DO.Customer()
            {
                Id = costumer.Id,
                Lattitude = costumer.Loct.Lattitude,
                Longitude = costumer.Loct.Longitude,
                Name = costumer.Name,
                Phone = costumer.Phone_Num
            };
            try
            {
                data.AddCustomer(CostumerTmp);
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCostumer(int costumerId, string costumerName = null, string costumerPhone = null)
        {
            try
            {
                DO.Customer customer = data.GetCustomer(costumerId);
                if (!(costumerName is null))
                {
                    customer.Name = costumerName;
                }

                if (!(costumerPhone is null))
                {
                    customer.Phone = costumerPhone;
                }

                data.UpdateCustomer(customer);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id)
        {
            try
            {
                data.DeleteCustomer(id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerList> GetCustomers()
        {
            return data.GetCustomers().Select(ConvertList);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerList> GetCostumersFiltered(IEnumerable<int>? reached, IEnumerable<int>? Unreched, IEnumerable<int>? ParcelGot, IEnumerable<int>? InTheWay)
        {
            return data.GetCustomers(x => (reached is null || reached.Contains(data.CountParcels(x => x.SenderId == x.Id && ParcelStatusC(x) == ParcelStatus.Delivered))) &&
                                    (Unreched is null || Unreched.Contains(data.CountParcels(x => x.SenderId == x.Id && ParcelStatusC(x) != ParcelStatus.Delivered))) &&
                                    (ParcelGot is null || ParcelGot.Contains(data.CountParcels(x => x.TargetId == x.Id && ParcelStatusC(x) == ParcelStatus.Delivered))) &&
                                    (InTheWay is null || InTheWay.Contains(data.CountParcels(x => x.TargetId == x.Id && ParcelStatusC(x) != ParcelStatus.Delivered))))
                    .Select(ConvertList);
        }
    }
}

using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public IEnumerable<CustomerList> SmartSearchCostumer(string query)
        {
            int id;
            if (int.TryParse(query, out id))
            {
                return data.GetCustomers(x => x.Id.ToString().StartsWith(id.ToString())).Select(s => ConvertList(s));
            }
            return data.GetCustomers(x => x.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase)).Select(ConvertList);
        }
        public IEnumerable<BO.ParcelList> SmartSearchParcel(string query)
        {
            int id;
            if (int.TryParse(query, out id))
            {
                return data.GetParcels(x => x.Id.ToString().StartsWith(id.ToString())).Select(ConvertList);
            }
            return data.GetParcels(x => data.GetCustomer(x.SenderId).Name.Contains(query, StringComparison.CurrentCultureIgnoreCase) || 
                                        data.GetCustomer(x.TargetId).Name.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                                        .Select(ConvertList);
        }
        public IEnumerable<BO.StationList> SmartSearchStation(string query)
        {
            int id;
            if (int.TryParse(query, out id))
            {
                return data.GetStations(x => x.Id.ToString().StartsWith(id.ToString())).Select(ConvertList);
            }
            return data.GetStations(x => x.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase)).Select(ConvertList);
        }
        public IEnumerable<BO.DroneList> SmartSearchDrone(string query)
        {
            int id; 
            if (int.TryParse(query, out id))
            {
                return drones.FindAll(x => x.Id.ToString().StartsWith(id.ToString()));
            }
            return drones.FindAll(x => x.Model.Contains(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
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
                return data.GetCustomers(x => x.Id == id).Select(s => ConvertList(s));
            }
            return data.GetCustomers(x => x.Name.Contains(query)).Select(s => ConvertList(s));
        }
        public IEnumerable<BO.ParcelList> SmartSearchParcel(string query)
        {
            int id;
            if (int.TryParse(query, out id))
            {
                return data.GetParcels(x => x.Id == id).Select(s => ConvertList(s));
            }
            return Enumerable.Empty<BO.ParcelList>();
        }
        public IEnumerable<BO.StationList> SmartSearchStation(string query)
        {
            int id;
            if (int.TryParse(query, out id))
            {
                return data.GetStations(x => x.Id == id).Select(s => ConvertList(s));
            }
            return data.GetStations(x => x.Name.Contains(query)).Select(s => ConvertList(s));
        }
        public IEnumerable<BO.DroneList> SmartSearchDrone(string query)
        {
            int id;
            if (int.TryParse(query, out id))
            {
                return drones.FindAll(x => x.Id == id);
            }
            return drones.FindAll(x => x.Model.Contains(query));
        }
    }
}
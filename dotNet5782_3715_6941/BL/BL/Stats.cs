using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DronesModelsStats GetDronesModelsStats()
        {
            DronesModelsStats res = new DronesModelsStats();

            IEnumerable<string> Models = from Drony in drones select Drony.Model;
            Models = Models.Distinct();
            res.names = Models.ToArray();

            IEnumerable<int> range = Enumerable.Range(0, Models.Count());
            res.pos = range.Select(System.Convert.ToDouble).ToArray();

            IEnumerable<double> vals = from model in Models select (double)drones.Count(x => x.Model == model);
            res.vals = vals.ToArray();

            return res;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetDronesStatusesStats()
        {
            BO.DroneStatuses[] statuses = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
            IEnumerable<double> filtered = from status in statuses
                                           where drones.Count(x => x.DroneStat == status) > 0
                                           select (double)drones.Count(x => x.DroneStat == status);
            return filtered.ToArray();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetDronesWeightsStats()
        {
            BO.WeightCategories[] weights = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
            IEnumerable<double> filtered = from weight in weights
                                           where drones.Count(x => x.Weight == weight) > 0
                                           select System.Convert.ToDouble(drones.Count(x => x.Weight == weight));
            return filtered.ToArray();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetParcelsPrioretiesStats()
        {
            BO.Priorities[] priorities = (BO.Priorities[])Enum.GetValues(typeof(BO.Priorities));
            IEnumerable<double> filtered = from priority in priorities
                                           where data.CountParcels(x => x.Priority == (DO.Priorities)priority) > 0
                                           select (double)data.CountParcels(x => x.Priority == (DO.Priorities)priority);
            return filtered.ToArray();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetParcelsStatusesStats()
        {
            BO.ParcelStatus[] statuses = (BO.ParcelStatus[])Enum.GetValues(typeof(BO.ParcelStatus));
            IEnumerable<double> filtered = from status in statuses
                                           where data.CountParcels(x => ParcelStatusC(x) == status) > 0
                                           select (double)data.CountParcels(x => ParcelStatusC(x) == status);
            return filtered.ToArray();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetParcelsWeightsStats()
        {
            BO.WeightCategories[] weights = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
            IEnumerable<double> filtered = from weight in weights
                                           where data.CountParcels(x => x.Weight == (DO.WeightCategories)weight) > 0
                                           select (double)data.CountParcels(x => x.Weight == (DO.WeightCategories)weight);
            return filtered.ToArray();
        }
        // public double[] GetStationBusyPortsStats();
        // public double[] GetStationFreePortsStats();
        // public double[] GetCostumerReached();
        // public double[] GetCostumerUnReached();
        // public double[] GetCostumerParcelGot();
        // public double[] GetCostumerInTheWay();
    }
}
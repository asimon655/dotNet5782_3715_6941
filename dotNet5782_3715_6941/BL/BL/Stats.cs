using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public DronesModelsStats GetDronesModelsStats()
        {
            DronesModelsStats res = new DronesModelsStats();

            IEnumerable<string> Models = from Drony in drones select Drony.Model;
            Models = Models.Distinct();
            res.names = Models.ToArray();

            IEnumerable<int> range = Enumerable.Range(0, Models.Count());
            double []res_dd = new double[range.Count()];
            for (int i = 0; i < range.Count(); i++)
                res_dd[i] = System.Convert.ToDouble(range.Skip(i).First());
            res.pos = res_dd;
            IEnumerable<double> vals = from model in Models select (double)drones.Count(x => x.Model == model);
            res.vals = vals.ToArray();

            return res;
        }

        public double[] GetDronesStatusesStats()
        {
            BO.DroneStatuses[] statuses = (BO.DroneStatuses[])Enum.GetValues(typeof(BO.DroneStatuses));
            IEnumerable<double> filtered = from status in statuses
                                           where drones.Count(x => x.DroneStat == status) > 0
                                           select (double)drones.Count(x => x.DroneStat == status);
            return filtered.ToArray();
        }

        public double[] GetDronesWeightsStats()
        {
            BO.WeightCategories[] weights = (BO.WeightCategories[])Enum.GetValues(typeof(BO.WeightCategories));
            IEnumerable<double> filtered = from weight in weights
                                           where drones.Count(x => x.Weight == weight) > 0
                                           select System.Convert.ToDouble(drones.Count(x => x.Weight == weight));
            return filtered.ToArray();
        }

        public double[] GetParcelsPrioretiesStats()
        {
            BO.Priorities[] priorities = (BO.Priorities[])Enum.GetValues(typeof(BO.Priorities));
            IEnumerable<double> filtered = from priority in priorities
                                           where data.CountParcels(x => x.Priority == (DO.Priorities)priority) > 0
                                           select (double)data.CountParcels(x => x.Priority == (DO.Priorities)priority);
            return filtered.ToArray();
        }

        public double[] GetParcelsStatusesStats()
        {
            BO.ParcelStatus[] statuses = (BO.ParcelStatus[])Enum.GetValues(typeof(BO.ParcelStatus));
            IEnumerable<double> filtered = from status in statuses
                                           where data.CountParcels(x => ParcelStatusC(x) == status) > 0
                                           select (double)data.CountParcels(x => ParcelStatusC(x) == status);
            return filtered.ToArray();
        }

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
using BO;
using System.Collections.Generic;



namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public IEnumerable<StationList> GetStationsWithFreePorts()
        {
            List<StationList> tmp = new List<StationList>();
            // loop all stations with free charging slots
            foreach (var station in data.GetStations(x => x.ChargeSlots > 0))
            {
                int numOfNotFr = data.CountDronesCharges(x => x.StaionId == station.Id);
                tmp.Add(new StationList() { Id = station.Id, Name = station.Name, FreePorts = station.ChargeSlots, BusyPorts = numOfNotFr });
            }
            return tmp;

        }

        public IEnumerable<StationList> GetStaions()
        {
            List<StationList> tmp= new List<StationList>();
            foreach (var station in data.GetStations())
            {
                int numOfNotFr = data.CountDronesCharges(x=>x.StaionId ==  station.Id);
                tmp.Add(new StationList() { Id = station.Id, Name = station.Name, FreePorts = station.ChargeSlots, BusyPorts = numOfNotFr  });
            }
            return tmp; 
        }
       

    }
}
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
                tmp.Add(ConvertList(station));
            }
            return tmp;

        }

        public IEnumerable<StationList> GetStations()
        {
            List<StationList> tmp= new List<StationList>();
            foreach (var station in data.GetStations())
            {
                tmp.Add(ConvertList(station));
            }
            return tmp; 
        }

    }
}
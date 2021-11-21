using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IBL
{
    namespace BO
    {
        public class BaseStation : Printable
        {
            public int Id { set; get; }
            public int Name { set; get; }
            public Location LoctConstant { get; init;  }
            public int NumOfFreeOnes { set; get; }
            public List<DroneInCharge> DroneInChargeList { set ; get; }

        }
    }
}
namespace BL
{
    public partial class Bl : IBL.Ibl
    {
       
        public void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null)
        {
            IDAL.DO.Station Stationy = new IDAL.DO.Station() { Id = stationId,Name=(int)stationName ,ChargeSlots= (int)stationChargeSlots - data.DronesChargesPrint().Count(x => x.StaionId == stationId) };
            data.UpdateStations(Stationy);
        }
        public BaseStation PullDataStaion(int stationId)
        {
            BaseStation TmpStation = StationC(data.PullDataStation(stationId));
            List<DroneInCharge> dronesInCharges = new List<DroneInCharge>();
            foreach (var droneCharge in data.DronesChargesPrint())
            {
                if (droneCharge.StaionId == stationId)
                {
                    DroneToList drone = drones.Find(s => s.Id == droneCharge.DroneId);
                    // check if the drone exsists
                    DroneInCharge droneInCharge = new DroneInCharge { id = drone.Id,
                                                                      BatteryStat = drone.BatteryStat };
                    dronesInCharges.Add(droneInCharge);
                }
            }
            

            return TmpStation;
        }

        public void AddStation(BaseStation station)
        {
            IDAL.DO.Station StationTmp = new IDAL.DO.Station() {Id = station.Id,ChargeSlots=station.NumOfFreeOnes,Lattitude=station.LoctConstant.Lattitude,Longitude=station.LoctConstant.Longitude
            ,Name=station.Name};
            data.AddStation(StationTmp);
         
        }

        List<IDAL.DO.Station> getStationsFreePorts()
        {
            IEnumerable<IDAL.DO.Station> stations = data.StationsPrint();
            List<IDAL.DO.Station> res = new List<IDAL.DO.Station>();
            foreach (var station in stations)
            {
                if (station.ChargeSlots > 0)
                {
                    res.Add(station);
                }
            }
            return res;
        }


       
       


    }
}

using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IBL
{
    namespace BO
    {
        public class Drone : Printable
        {
            public int Id { set; get;  }
            public String Model { set; get; }
            public WeightCategories Weight { set; get; }
            public double BatteryStat { set; get;  }
            public DroneStatuses DroneStat { set; get;  }
            public IBL.BO.ParcelInTransfer ParcelTransfer { set; get;  }
            public Location Current { set; get;  }

        }
    }
}
namespace BL
{
    public partial class Bl : IBL.Ibl
    {


        public IEnumerable<DroneToList> DronesPrint()
        {
            return drones;
        }
        public void DroneChargeRelease(int droneId, double chargingPeriod)
        {
            if (chargingPeriod < 0)
                throw new NotImplementedException(); 
            DroneToList drony = drones.Find(x => x.Id == droneId);
            if (drony.DroneStat == DroneStatuses.Matance)
            {
                ///time perios is in secs 
                IDAL.DO.Station  ? station = GetStationFromCharging(droneId);
                IDAL.DO.Station stationreal = (IDAL.DO.Station)station;
                if (station == null)
                    throw new NotImplementedException();
                double chargingSpeed = data.GetPowerConsumption()[4];
                drony.BatteryStat = (chargingSpeed *(chargingPeriod/60*60) > 100 ? 100 :chargingSpeed * (chargingPeriod /  60 * 60) ); 
                drony.DroneStat = DroneStatuses.Free; 
                BaseStation baseStation = StationC(stationreal);
                double powerUsage = getPowerUsage(drony.Current, baseStation.LoctConstant);
                stationreal.ChargeSlots += 1;
                data.UpdateStations(stationreal);
                data.DeleteDroneCharge(drony.Id);

            }
        }

        public void DroneCharge(int droneId)
        {
            DroneToList drony = drones.Find(x => x.Id == droneId);
            if (drony.DroneStat == DroneStatuses.Free)
            {
                int stationID = getClosesStation(drony.Current);
                if (stationID == 0)
                    throw new NotImplementedException();
                IDAL.DO.Station station = data.PullDataStation(stationID);
                BaseStation baseStation = StationC(station);
                double powerUsage = getPowerUsage(drony.Current, baseStation.LoctConstant);
                if (drony.BatteryStat >= powerUsage )
                {
                    drony.DroneStat = DroneStatuses.Matance;
                    drony.Current = baseStation.LoctConstant;
                    drony.BatteryStat -= powerUsage;
                }
                station.ChargeSlots -= 1;
                data.UpdateStations(station);
                IDAL.DO.DroneCharge chargingport = new IDAL.DO.DroneCharge() { DroneId =drony.Id ,StaionId =station.Id};
                data.AddDroneCharge(chargingport);

            }        
        }
        public void UpdateDrone(int droneId, string droneName)
        {
            IDAL.DO.Drone Drony= new IDAL.DO.Drone() { Id=droneId , Modle=droneName};
            data.UpdateDrones(Drony);
            drones.Find(x => x.Id == droneId).Model = droneName; 
        }


        public Drone PullDataDrone(int id)
        {
            return DronesC(data.PullDataDrone(id));
        }
        public void AddDrone(Drone drone, int stationId)
        {
            IDAL.DO.Drone DroneTmp = new IDAL.DO.Drone() { Id = drone.Id, MaxWeigth = ((IDAL.DO.WeightCategories)(int)drone.Weight), Modle = drone.Model };
            data.AddDrone(DroneTmp);
            data.AddDroneCharge(new IDAL.DO.DroneCharge { StaionId = stationId, DroneId = drone.Id });
            drone.BatteryStat = RandomGen.NextDouble() * 20 + 20;
            drone.DroneStat = IBL.BO.DroneStatuses.Matance;
            IDAL.DO.Station PulledStaion = data.PullDataStation(stationId);
            drone.Current = new Location(PulledStaion.Longitude, PulledStaion.Lattitude) ;
            DroneToList TmpDrnLst = new DroneToList() {
                BatteryStat=drone.BatteryStat ,
                Id=drone.Id , 
                Current=drone.Current , 
                DroneStat=drone.DroneStat , 
                Model=drone.Model ,
                ParcelIdTransfer=null, /// just created there is no reason that parcel will binded toit already   
                Weight=drone.Weight};
            drones.Add(TmpDrnLst);
       

        }
    }
}

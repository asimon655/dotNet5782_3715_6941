using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IBL
{
    namespace BO
    {
        public class Drone 
        {
            public int Id { set; get;  }
            public String Model { set; get; }
            public WeightCategories Weight { set; get; }
            public double BatteryStat { set; get;  }
            public DroneStatuses DroneStat { set; get;  }
            public IBL.BO.ParcelInTransfer? ParcelTransfer { set; get;  } = null;
            public Location Current { set; get;  }

            public override string ToString()
            {
                return $"Id : {Id}\n" +
                       $"Model : {Model}\n" +
                       $"location : {Current}\n" +
                       $"battary : {BatteryStat}\n" +
                       $"Max Weight : {Weight}\n" +
                       $"Drone Status : {DroneStat}\n" +
                       $"binded parcele : {ParcelTransfer}";
            }
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
                throw new NotValidTimePeriod("you tried to enter time that is smaller than 0 (forbidden) ",chargingPeriod); 
            
            DroneToList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Matance)
                throw new EnumNotInRightStatus<DroneStatuses>("drone is not in matance , it is: ", drony.DroneStat); 
            else 
            {
                ///time perios is in hours 
                IDAL.DO.Station station = GetStationFromCharging(droneId);
                drony.BatteryStat = Math.Min((drony.BatteryStat + ChargingSpeed * chargingPeriod), 100); 
                drony.DroneStat = DroneStatuses.Free; 
                BaseStation baseStation = StationC(station);
                station.ChargeSlots += 1;
                try
                {
                    data.UpdateStations(station); 
                    data.DeleteDroneCharge(drony.Id);
                }
                catch (IDAL.DO.IdDosntExists err)
                {
                    throw new IdDosntExists(err); 
                    
                }
         
                   
          

            }
        }

        public void DroneCharge(int droneId)
        {
            DroneToList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Free)
                throw new EnumNotInRightStatus<DroneStatuses>("drone is not not Free  , it is :  ", drony.DroneStat);
            else
            {
                try
                {
                    int stationID = getClosesStation(drony.Current);
                    IDAL.DO.Station station = data.PullDataStation(stationID);
                    Location stationLoct = new Location(station.Longitude, station.Lattitude);
                    double powerUsage = getPowerUsage(drony.Current, stationLoct);
                    if (drony.BatteryStat < powerUsage)
                        throw new CantReachToDest("the charging port os too far to go with the current battery precantage", drony.BatteryStat, powerUsage);
                    drony.DroneStat = DroneStatuses.Matance;
                    drony.Current = stationLoct;
                    drony.BatteryStat -= powerUsage;
                    station.ChargeSlots -= 1;


                    IDAL.DO.DroneCharge chargingport = new IDAL.DO.DroneCharge() { DroneId = drony.Id, StaionId = station.Id };

                    data.AddDroneCharge(chargingport);
                }
                catch (IDAL.DO.IdAlreadyExists err)
                {
                    throw new IdAlreadyExists(err);
                }
                catch (IDAL.DO.IdDosntExists err)
                {
                    throw new IdDosntExists(err);
                }
            }     
        }
        public void UpdateDrone(int droneId, string droneName)
        {
            
            IDAL.DO.Drone Drony= new IDAL.DO.Drone() { 
                Id=droneId , 
                Modle=droneName ,
                MaxWeigth=data.PullDataDrone(droneId).MaxWeigth
           };
            try
            {
                data.UpdateDrones(Drony);
                GetDroneToList(droneId).Model = droneName;
            }
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
                
            } 
            
            
        }


        public Drone PullDataDrone(int id)
        {
            return DronesC(GetDroneToList(id));
        }
        public void AddDrone(Drone drone, int stationId)
        {
            isInEnum<DroneStatuses>(drone.DroneStat);
            isInEnum<WeightCategories>(drone.Weight); 
            IDAL.DO.Drone DroneTmp = new IDAL.DO.Drone() { 
                Id = drone.Id, 
                MaxWeigth = ((IDAL.DO.WeightCategories)(int)drone.Weight), 
                Modle = drone.Model };
            try
            {
                data.AddDrone(DroneTmp);
                data.AddDroneCharge(new IDAL.DO.DroneCharge { StaionId = stationId, DroneId = drone.Id });
            }
            catch (IDAL.DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err); 
            } 
            drone.BatteryStat = RandomGen.NextDouble() * 20 + 20;
            drone.DroneStat = IBL.BO.DroneStatuses.Matance;
            try
            {
                IDAL.DO.Station PulledStaion = data.PullDataStation(stationId);
                drone.Current = new Location(PulledStaion.Longitude, PulledStaion.Lattitude);
            }
            catch (IDAL.DO.IdDosntExists err) {


                throw new IdDosntExists(err);
            
            } 
            
            DroneToList TmpDrnLst = new DroneToList() {
                BatteryStat=drone.BatteryStat ,
                Id=drone.Id , 
                Current=drone.Current , 
                DroneStat=drone.DroneStat , 
                Model=drone.Model ,   
                Weight=drone.Weight};
            try
            {
                drones.Add(TmpDrnLst);
            }
            catch (IDAL.DO.IdAlreadyExists err) {


                throw new IdAlreadyExists(err); 
            }  
       

        }
    }
}

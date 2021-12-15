using BO;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {


        public IEnumerable<DroneList> GetDrones()
        {
            return drones;
        }
        public void DroneReleaseCharge(int droneId, double chargingPeriod)
        {
            if (chargingPeriod < 0)
                throw new NotValidTimePeriod("you tried to enter time that is smaller than 0 (forbidden) ",chargingPeriod); 
            
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Matance)
                throw new EnumNotInRightStatus<DroneStatuses>("drone is not in matance , it is: ", drony.DroneStat); 
            else 
            {
                ///time perios is in hours 
                DO.Station station = GetStationFromCharging(droneId);
                drony.Battery = Math.Min((drony.Battery + ChargingSpeed * chargingPeriod), 100); 
                drony.DroneStat = DroneStatuses.Free; 
                Station baseStation = StationC(station);
                station.ChargeSlots += 1;
                try
                {
                    data.UpdateStations(station); 
                    data.DeleteDroneCharge(drony.Id);
                }
                catch (DO.IdDosntExists err)
                {
                    throw new IdDosntExists(err); 
                    
                }
         
                   
          

            }
        }

        public void DroneCharge(int droneId)
        {
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Free)
                throw new EnumNotInRightStatus<DroneStatuses>("drone is not not Free  , it is :  ", drony.DroneStat);
            else
            {
                try
                {
                    int stationID = getClosesStation(drony.Loct);
                    DO.Station station = data.GetStation(stationID);
                    Location stationLoct = new Location(station.Longitude, station.Lattitude);
                    double powerUsage = getPowerUsage(drony.Loct, stationLoct);
                    if (drony.Battery < powerUsage)
                        throw new CantReachToDest("the charging port os too far to go with the current battery precantage", drony.Battery, powerUsage);
                    drony.DroneStat = DroneStatuses.Matance;
                    drony.Loct = stationLoct;
                    drony.Battery -= powerUsage;
                    station.ChargeSlots -= 1;


                    DO.DroneCharge chargingport = new DO.DroneCharge() { DroneId = drony.Id, StaionId = station.Id };

                    data.AddDroneCharge(chargingport);
                }
                catch (DO.IdAlreadyExists err)
                {
                    throw new IdAlreadyExists(err);
                }
                catch (DO.IdDosntExists err)
                {
                    throw new IdDosntExists(err);
                }
            }     
        }
        public void UpdateDrone(int droneId, string droneName)
        {
            
            DO.Drone Drony= new DO.Drone() { 
                Id=droneId , 
                Modle=droneName ,
                MaxWeigth=data.GetDrone(droneId).MaxWeigth
           };
            try
            {
                data.UpdateDrones(Drony);
                GetDroneToList(droneId).Model = droneName;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
                
            } 
            
            
        }


        public Drone GetDrone(int id)
        {
            return DronesC(GetDroneToList(id));
        }
        public void AddDrone(Drone drone, int stationId)
        {
            isInEnum<DroneStatuses>(drone.DroneStat);
            isInEnum<WeightCategories>(drone.Weight); 
            DO.Drone DroneTmp = new DO.Drone() { 
                Id = drone.Id, 
                MaxWeigth = ((DO.WeightCategories)(int)drone.Weight), 
                Modle = drone.Model };
            try
            {
                DO.Station PulledStaion = data.GetStation(stationId);
                drone.Current = new Location(PulledStaion.Longitude, PulledStaion.Lattitude);
            }
            catch (DO.IdDosntExists err) {


                throw new IdDosntExists(err);
            
            } 
            try
            {
                data.AddDrone(DroneTmp);
                data.AddDroneCharge(new DO.DroneCharge { StaionId = stationId, DroneId = drone.Id });
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err); 
            } 
            drone.BatteryStat = RandomGen.NextDouble() * 20 + 20;
            drone.DroneStat = DroneStatuses.Matance;
            
            DroneList TmpDrnLst = new DroneList() {
                Battery=drone.BatteryStat ,
                Id=drone.Id , 
                Loct=drone.Current , 
                DroneStat=drone.DroneStat , 
                Model=drone.Model ,   
                Weight=drone.Weight};
            try
            {
                drones.Add(TmpDrnLst);
            }
            catch (DO.IdAlreadyExists err) {


                throw new IdAlreadyExists(err); 
            }
       


        }
        public IEnumerable<DroneList> GetDronesFiltered(IEnumerable<DroneStatuses> statuses, IEnumerable<WeightCategories> weights)
        {
            return drones.Where(x => statuses.Contains(x.DroneStat) && weights.Contains(x.Weight));

        }
        public Predicate<DO.Station> conv(Predicate<StationList> x)
        {
            return new Predicate<DO.Station>(y => x(new StationList() { Id = y.Id, Name = y.Name, FreePorts = y.ChargeSlots }));  
        
        
        } 
    }
}

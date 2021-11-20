using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class Bl : IBL.Ibl
    {
        IDAL.Idal data = new DAL.DalObject.DalObject();

        List<DroneToList> drones = new List<DroneToList>();

        internal Random RandomGen = new Random();

        double PowerConsumptionFree;
        double PowerConsumptionLight;
        double PowerConsumptionMedium;
        double PowerConsumptionHeavy;
        double ChargingSpeed;

        public Bl()
        {
            // initilazing power related consts
            double[] powerConst = data.GetPowerConsumption();

            PowerConsumptionFree = powerConst[0];
            PowerConsumptionLight = powerConst[1];
            PowerConsumptionMedium = powerConst[2];
            PowerConsumptionHeavy = powerConst[3];
            ChargingSpeed = powerConst[4];

            // initilazing the drones list
            foreach (var drone in data.DronesPrint())
            {
                DroneToList newDrone = new DroneToList();
                newDrone.Id = drone.Id;
                newDrone.Model = drone.Modle;
                newDrone.Weight = (WeightCategories)drone.MaxWeigth;

                int parcelId = getBindedUndeliveredParcel(drone.Id);

                if (parcelId != -1) // the drone is binded
                {
                    newDrone.DroneStat = DroneStatuses.Delivery;
                    newDrone.ParcelIdTransfer = parcelId;
                    // get the binded parcel
                    IDAL.DO.Parcel parcel = data.PullDataParcel(parcelId);

                    // get the sender and target and there location
                    IDAL.DO.Costumer sender = data.PullDataCostumer(parcel.SenderId);
                    Location senderLoct = new Location(sender.Longitude, sender.Lattitude);
                    IDAL.DO.Costumer target = data.PullDataCostumer(parcel.TargetId);
                    Location targetLoct = new Location(target.Longitude, target.Lattitude);

                    // the parcel has been binded but not picked up yet
                    if (parcel.PickedUp == DateTime.MinValue)
                    {
                        // set the location of the drone to the closest station to the sender
                        int senderStationId = getClosesStation(senderLoct);
                        IDAL.DO.Station senderStation = data.PullDataStation(senderStationId);
                        Location senderStationLoct = new Location(senderStation.Longitude, senderStation.Lattitude);
                        newDrone.Current = senderStationLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        IDAL.DO.Station targetStation = data.PullDataStation(targetStationId);
                        Location targetStationLoct = new Location(targetStation.Longitude, targetStation.Lattitude);
                        // calculate the minimum battery this trip will take
                        double minimumBattery = 0;
                        minimumBattery += getPowerUsage(newDrone.Current, senderLoct);
                        minimumBattery += getPowerUsage(senderLoct, targetLoct, (WeightCategories?)parcel.Weight);
                        minimumBattery += getPowerUsage(targetLoct, targetStationLoct);
                        // set the drone battery randomly between the minimumBattery and 100%
                        newDrone.BatteryStat = 100D - RandomGen.NextDouble() * minimumBattery;
                    }
                    // the parcel has been binded and picked up but hasnt been deliverd yet
                    else
                    {
                        // set the location of the drone to the location of the sender
                        newDrone.Current = senderLoct;

                        int targetStationId = getClosesStation(targetLoct);
                        IDAL.DO.Station targetStation = data.PullDataStation(targetStationId);
                        Location targetStationLoct = new Location(targetStation.Longitude, targetStation.Lattitude);
                        // calculate the minimum battery this trip will take
                        double minimumBattery = 0;
                        minimumBattery += getPowerUsage(newDrone.Current, targetLoct, (WeightCategories?)parcel.Weight);
                        minimumBattery += getPowerUsage(targetLoct, targetStationLoct);
                        // set the drone battery randomly between the minimumBattery and 100%
                        newDrone.BatteryStat = 100D - RandomGen.NextDouble() * minimumBattery;
                    }
                }
                else // the drone is not binded
                {
                    // set DroneStat to a random value between Free, Matance
                    newDrone.DroneStat = (DroneStatuses)RandomGen.Next((int)DroneStatuses.Free, (int)DroneStatuses.Matance);
                }
                if (newDrone.DroneStat == DroneStatuses.Matance)
                {
                    // set drone location to a random starion with free charging slots
                    List<IDAL.DO.Station> stationsFreePorts = getStationsFreePorts();
                    IDAL.DO.Station station = stationsFreePorts[RandomGen.Next(stationsFreePorts.Count)];
                    newDrone.Current = new Location(station.Longitude, station.Lattitude);
                    // register the matance in drone charge
                    data.AddDroneCharge(new IDAL.DO.DroneCharge { StaionId = station.Id, DroneId = newDrone.Id });
                    // set drone battery
                    newDrone.BatteryStat = RandomGen.NextDouble() * 20; 
                }
                if (newDrone.DroneStat == DroneStatuses.Free)
                {
                    // set drone location to a random customer that recived a message
                    List<IDAL.DO.Parcel> parcelsDelivered = getDeliverdParcels();
                    IDAL.DO.Parcel parcel = parcelsDelivered[RandomGen.Next(parcelsDelivered.Count)];
                    IDAL.DO.Costumer costumer = data.PullDataCostumer(parcel.TargetId);
                    newDrone.Current = new Location(costumer.Longitude, costumer.Lattitude);

                    // get the location of the closest station to that costumer
                    int stationId = getClosesStation(newDrone.Current);
                    IDAL.DO.Station station = data.PullDataStation(stationId);
                    Location stationLoct = new Location(station.Longitude, station.Lattitude);

                    // calculate the minimum battery this trip will take
                    double minimumBattery = 0;
                    minimumBattery += getPowerUsage(newDrone.Current, stationLoct);
                    // set the drone battery randomly between the minimumBattery and 100%
                    newDrone.BatteryStat = 100D - RandomGen.NextDouble() * minimumBattery;
                }
                // add newDrone to drones list
                drones.Add(newDrone);
            }
        }
        // return the id of the parcel that binded to a specific drone and the parcel not yet delivered
        // if there isnt any return -1
        int getBindedUndeliveredParcel(int droneId)
        {
            foreach (var parcel in data.ParcelsPrint())
            {
                if (parcel.DroneId == droneId && parcel.Delivered == DateTime.MinValue)
                {
                    return parcel.Id;
                }
            }
            return -1;
        }
        // calculate distance between two locations
        double calculateDistance(Location location1, Location location2)
        {
            double rlat1 = Math.PI * location1.Lattitude / 180;
            double rlat2 = Math.PI * location2.Lattitude / 180;
            double theta = location1.Longitude - location2.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return Math.Round(dist * 1.609344, 2);
        }
        // return the closes station to a given location
        int getClosesStation(Location location)
        {
            int stationId = 0;
            double shortestDistance = double.MaxValue;
            foreach (var station in data.StationsPrint())
            {
                double distance = calculateDistance(new Location(station.Longitude, station.Lattitude), location);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    stationId = station.Id;
                }
            }
            return stationId;
        }

        double getPowerUsage(Location from, Location to, WeightCategories? weight = null)
        {
            switch (weight)
            {
                case WeightCategories.Easy:
                    return calculateDistance(from, to) * PowerConsumptionLight;
                case WeightCategories.Medium:
                    return calculateDistance(from, to) * PowerConsumptionMedium;
                case WeightCategories.Heavy:
                    return calculateDistance(from, to) * PowerConsumptionHeavy;
                default: // the drone is free
                    return calculateDistance(from, to) * PowerConsumptionFree;
            }
        }
        // return a list of stations with free charging slots
        // (this is a help function so its useful to return list than ienumerable)
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
        List<IDAL.DO.Parcel> getDeliverdParcels()
        {
            IEnumerable<IDAL.DO.Parcel> parcels = data.ParcelsPrint();
            List<IDAL.DO.Parcel> res = new List<IDAL.DO.Parcel>();
            foreach (var parcel in parcels)
            {
                if (parcel.Delivered != DateTime.MinValue)
                {
                    res.Add(parcel);
                }
            }
            return res;
        }
        public void AddCostumer(Costumer costumer)
        {
            IDAL.DO.Costumer CostumerTmp = new IDAL.DO.Costumer() { Id = costumer.Id, Lattitude = costumer.Loct.Lattitude, Longitude = costumer.Loct.Longitude, Name = costumer.Name, Phone = costumer.Phone_Num };
            data.AddCostumer(CostumerTmp);
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
            DroneToList TmpDrnLst = new DroneToList() {BatteryStat=drone.BatteryStat , Id=drone.Id , Current=drone.Current , DroneStat=drone.DroneStat , Model=drone.Model ,ParcelIdTransfer=drone.ParcelTransfer.Id , Weight=drone.Weight};
            drones.Add(TmpDrnLst);
       

        }

        public void AddParcel(Parcel parcel)
        {
            IDAL.DO.Parcel ParcelTmp = new IDAL.DO.Parcel() { Id = parcel.Id,Delivered=DateTime.MinValue ,PickedUp= DateTime.MinValue, Requested= DateTime.MinValue, Schedulded= DateTime.MinValue, SenderId=parcel.SenderParcelToCostumer.id,TargetId=parcel.GetterParcelToCostumer.id , DroneId =null , Priority=(IDAL.DO.Priorities)parcel.Priority,Weight=(IDAL.DO.WeightCategories)parcel.Weight};
            data.AddParcel(ParcelTmp);
           

        }

        public void AddStation(BaseStation station)
        {
            IDAL.DO.Station StationTmp = new IDAL.DO.Station() {Id = station.Id,ChargeSlots=station.NumOfFreeOnes,Lattitude=station.LoctConstant.Lattitude,Longitude=station.LoctConstant.Longitude
            ,Name=station.Name};
            data.AddStation(StationTmp);
         
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

        public Costumer PullDataCostumer(int id)
        {
            return CostumerC(data.PullDataCostumer(id));
            throw new NotImplementedException();
        }

        public Drone PullDataDrone(int id)
        {
            return DronesC(data.PullDataDrone(id));
            throw new NotImplementedException();
        }

        public Parcel PullDataParcel(int id)
        {
            return ParcelC(data.PullDataParcel(id));
            throw new NotImplementedException();
        }

        public void UpdateDrone(int droneId, string droneName)
        {
            IDAL.DO.Drone Drony= new IDAL.DO.Drone() { Id=droneId , Modle=droneName};
            data.UpdateDrones(Drony);
            drones.Find(x => x.Id == droneId).Model = droneName; 
        }

        public void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null)
        {
            IDAL.DO.Station Stationy = new IDAL.DO.Station() { Id = stationId,Name=(int)stationName ,ChargeSlots= (int)stationChargeSlots - data.DronesChargesPrint().Count(x => x.StaionId == stationId) };
            data.UpdateStations(Stationy);
        }

        public void UpdateCostumer(int costumerId, string costumerName = null, string costumerPhone = null)
        {
            IDAL.DO.Costumer Costumery = new IDAL.DO.Costumer() { Id = costumerId,Name=costumerName  ,Phone=costumerPhone };
            data.UpdateCostumers(Costumery);
        }
        private  Location getParcelLoctSender(IDAL.DO.Parcel parcel)
        {
            foreach (var costumer in data.CostumersPrint())
                if (costumer.Id == parcel.SenderId)
                    return CostumerC(costumer).Loct;
            throw new NotImplementedException();
        }
        private  Location getParcelLoctTarget(IDAL.DO.Parcel parcel)
        {
            foreach (var costumer in data.CostumersPrint())
                if (costumer.Id == parcel.TargetId)
                    return CostumerC(costumer).Loct;
            throw new NotImplementedException();
        }
        private bool canreach(DroneToList drony , IDAL.DO.Parcel parcel , Func<IDAL.DO.Parcel,Location> function)
        {

            return getPowerUsage(drony.Current, function(parcel), (WeightCategories)parcel.Weight) < drony.BatteryStat; 
        
        }
        public void BindParcelToDrone(int droneId)
        {
            IEnumerable<IDAL.DO.Parcel> list = data.ParcelsPrint();
            IDAL.DO.Parcel  resParcel = list.First();
            DroneToList drony = drones.Find(x => x.Id == droneId);
            foreach (var pack in data.ParcelsPrint())
                if (canreach(drony, pack, getParcelLoctSender))
                    if (pack.Requested == DateTime.MinValue)
                    {
                        if (pack.Priority > resParcel.Priority)
                            resParcel = pack;
                        else
                        {
                            if (pack.Priority == resParcel.Priority)
                            {
                                if ((int)pack.Weight <= (int)drony.Weight && pack.Weight > resParcel.Weight)
                                    resParcel = pack;
                                else
                                {
                                    if ((int)pack.Weight <= (int)drony.Weight && pack.Weight == resParcel.Weight)
                                    {
                                        if (calculateDistance(drony.Current, getParcelLoctSender(pack)) < calculateDistance(drony.Current, getParcelLoctSender(resParcel)))
                                            resParcel = pack;
                                    }
                                } 

                            }
                        } 
                    } 
            drony.ParcelIdTransfer = resParcel.Id;
            resParcel.Schedulded = DateTime.Now;
            drony.DroneStat = DroneStatuses.Delivery;
            data.UpdateParcles(resParcel);
            
            

        }

        public void PickUpByDrone(int droneId)
        {
            DroneToList drony = drones.Find(x => x.Id == droneId);
            IDAL.DO.Parcel pack = data.PullDataParcel((int)drony.ParcelIdTransfer);
            if (drony.ParcelIdTransfer != null || pack.PickedUp == DateTime.MinValue)
            {
                if (!canreach(drony, pack,getParcelLoctSender))
                    throw new NotImplementedException();
                ///battery status changed !!! 
                drony.BatteryStat -= getPowerUsage(getParcelLoctSender(pack), drony.Current);
                drony.Current = getParcelLoctSender(pack);
                pack.PickedUp = DateTime.Now;
                data.UpdateParcles(pack);


            }
            else
                throw new NotImplementedException();

        }

        public void ParcelDeliveredToCostumer(int droneId)
        {
            DroneToList drony = drones.Find(x => x.Id == droneId);
            IDAL.DO.Parcel pack = data.PullDataParcel((int)drony.ParcelIdTransfer);
            Location Target = getParcelLoctTarget(pack);
            if (drony.ParcelIdTransfer != null || pack.Delivered == DateTime.MinValue)
            {
                if (!canreach(drony, pack, getParcelLoctTarget))
                    throw new NotImplementedException();
                drony.BatteryStat = getPowerUsage(Target, drony.Current, (WeightCategories)pack.Weight);
                drony.Current = Target;
                drony.DroneStat = DroneStatuses.Free;
                pack.Delivered = DateTime.Now;
                data.UpdateParcles(pack);

            }


            else
                throw new NotImplementedException();

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
        private IDAL.DO.Station  ? GetStationFromCharging(int droneId)
        {
            foreach (var charges in data.DronesChargesPrint())
                if (charges.DroneId == droneId)
                    return data.PullDataStation(charges.StaionId);
            return null; 
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

    

    public IEnumerable<BaseStaionToList> StaionsPrint()
        {
            List<BaseStaionToList> tmp= new List<BaseStaionToList>();
            foreach (var station in data.StationsPrint())
            {
                int numOfNotFr = data.DronesChargesPrint().Count(x=>x.StaionId==  station.Id);
                tmp.Add(new BaseStaionToList() { Id = station.Id, Name = station.Name, NumOfFreeOnes = station.ChargeSlots, NumOfNotFreeOne = numOfNotFr  });
            }
            return tmp; 
        }

        public IEnumerable<BaseStaionToList> BaseStaionsFreePortsPrint()
        {
            List<BaseStaionToList> tmp = new List<BaseStaionToList>();
            foreach (var station in data.StationsPrint())
            {
                int numOfNotFr = data.DronesChargesPrint().Count(x => x.StaionId == station.Id);
                if (station.ChargeSlots > 0 ) 
                    tmp.Add(new BaseStaionToList() { Id = station.Id, Name = station.Name, NumOfFreeOnes = station.ChargeSlots, NumOfNotFreeOne = numOfNotFr });
            }
            return tmp;

        }
        private ClientToList CltToLstC(IDAL.DO.Costumer gety)
        {
            return new ClientToList() { Id = gety.Id, Name=gety.Name 
                , Phone=gety.Phone 
                ,ParcelDeliveredAndGot =data.ParcelsPrint().Count (x => x.SenderId == gety.Id && x.PickedUp != DateTime.MinValue)  
                , InTheWay= data.ParcelsPrint().Count(x => x.SenderId == gety.Id && x.Schedulded != DateTime.MinValue && x.Delivered == DateTime.MinValue)
                , ParcelGot = data.ParcelsPrint().Count(x => x.TargetId == gety.Id && x.Delivered != DateTime.MinValue) 
                , ParcelDeliveredAndNotGot = data.ParcelsPrint().Count(x => x.SenderId == gety.Id && x.Delivered != DateTime.MinValue && x.PickedUp == DateTime.MinValue) };

        }


        public IEnumerable<ClientToList> CostumersPrint()
        {
            List<ClientToList> tmpy = new List<ClientToList>();
            foreach(var x in data.CostumersPrint())
            {
                tmpy.Add(CltToLstC(x));
            }
            return tmpy;
        }
        

        public IEnumerable<DroneToList> DronesPrint()
        {
            return drones;
        }
        private ParcelStat ParcelStatC(IDAL.DO.Parcel parcel)
        {
            int caseNum = 4;
            if (parcel.PickedUp != DateTime.MinValue)
                caseNum--; 
            else if (parcel.Delivered != DateTime.MinValue)
                caseNum--;
            else if (parcel.Schedulded!= DateTime.MinValue)
                caseNum--;
            else if (parcel.Requested != DateTime.MinValue)
                caseNum--;
            if (caseNum == 4)
                throw new NotImplementedException(); 
            return (ParcelStat)caseNum; 
        } 
        public IEnumerable<ParcelToList> ParcelsPrint()
        {
            List<ParcelToList> tmpy = new List<ParcelToList>();
            data.ParcelsPrint().ToList().ForEach(x => tmpy.Add( new ParcelToList() { Id = x.Id 
                , ParcelStatus = ParcelStatC(x) 
                , Priorety =(Priorities)x.Priority 
                ,SenderName= data.CostumersPrint().ToList().Find(y =>y.Id == x.SenderId ).Name
                , TargetName= data.CostumersPrint().ToList().Find(y => y.Id == x.TargetId).Name
                , Weight=  (WeightCategories)x.Weight
            } ));
            return tmpy;
            throw new NotImplementedException();
        }

        public IEnumerable<ParcelToList> ParcelsWithoutDronesPrint()
        {
            List<ParcelToList> tmpy = new List<ParcelToList>();
            data.ParcelsPrint().ToList().FindAll(y => y.Schedulded == DateTime.MinValue).ForEach(x => tmpy.Add(new ParcelToList()
            {
                Id = x.Id
                ,
                ParcelStatus = ParcelStatC(x)
                ,
                Priorety = (Priorities)x.Priority
                ,
                SenderName = data.CostumersPrint().ToList().Find(y => y.Id == x.SenderId).Name
                ,
                TargetName = data.CostumersPrint().ToList().Find(y => y.Id == x.TargetId).Name
                ,
                Weight = (WeightCategories)x.Weight
            }));

            return tmpy;
            throw new NotImplementedException();
        }

        Drone DronesC(IDAL.DO.Drone drone)
        {
            return new Drone() { Id = drone.Id, Weight = (WeightCategories)drone.MaxWeigth, Model = drone.Modle };

        }
        BaseStation StationC(IDAL.DO.Station station)
        {
            return new BaseStation()
            {
                Id = station.Id,
                NumOfFreeOnes = station.ChargeSlots,
                LoctConstant = new Location(station.Longitude, station.Lattitude),
                Name = station.Name
            };
        }
        Costumer CostumerC(IDAL.DO.Costumer costumer)
        {
            return new Costumer() { Id = costumer.Id, Loct = new Location(costumer.Longitude, costumer.Lattitude), Name = costumer.Name, Phone_Num = costumer.Phone };

        }
        Parcel ParcelC(IDAL.DO.Parcel parcel)
        {
            Parcel ParcelTmp = new Parcel() { Id = parcel.Id, ParcelDelivered = parcel.Delivered, ParcelPickedUp = parcel.PickedUp, ParcelCreation = parcel.Requested, ParcelBinded = parcel.Schedulded, Priority = (Priorities)parcel.Priority, Weight = (WeightCategories)parcel.Weight };
            ParcelTmp.SenderParcelToCostumer.id = parcel.SenderId;
            ParcelTmp.GetterParcelToCostumer.id = parcel.TargetId;
            return ParcelTmp;

        }
    }
}

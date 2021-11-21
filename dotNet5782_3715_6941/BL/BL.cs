using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class Bl : IBL.Ibl
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
      

        private  Location getParcelLoctSender(IDAL.DO.Parcel parcel) 
            => CostumerC(data.CostumersPrint().ToList().Find(x => x.Id == parcel.SenderId)).Loct;  
        
        
        private Location getParcelLoctTarget(IDAL.DO.Parcel parcel) 
            => CostumerC(data.CostumersPrint().ToList().Find(x => x.Id == parcel.TargetId)).Loct;  
        
        
        private bool canreach(DroneToList drony , IDAL.DO.Parcel parcel , Func<IDAL.DO.Parcel,Location> function) 
            => getPowerUsage(drony.Current, function(parcel), (WeightCategories)parcel.Weight) <= drony.BatteryStat; 
        

        private IDAL.DO.Station  ? GetStationFromCharging(int droneId)
        {
            foreach (var charges in data.DronesChargesPrint())
                if (charges.DroneId == droneId)
                    return data.PullDataStation(charges.StaionId);
            return null; 
        } 
    

        private ClientToList CltToLstC(IDAL.DO.Costumer gety) =>  new ClientToList() { 
                Id = gety.Id 
                , Name=gety.Name 
                , Phone=gety.Phone 
                ,ParcelDeliveredAndGot =data.ParcelsPrint().Count (x => x.SenderId == gety.Id && x.PickedUp != DateTime.MinValue)  
                , InTheWay= data.ParcelsPrint().Count(x => x.SenderId == gety.Id && x.Schedulded != DateTime.MinValue && x.Delivered == DateTime.MinValue)
                , ParcelGot = data.ParcelsPrint().Count(x => x.TargetId == gety.Id && x.Delivered != DateTime.MinValue) 
                , ParcelDeliveredAndNotGot = data.ParcelsPrint().Count(x => x.SenderId == gety.Id && x.Delivered != DateTime.MinValue && x.PickedUp == DateTime.MinValue) };


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


        private Drone DronesC(IDAL.DO.Drone drone)
        {
            
            Parcel ?  parcely = PullDataParcel( data.ParcelsPrint().ToList().Find(x => x.DroneId == drone.Id).Id  );
            IDAL.DO.Costumer Sender = data.CostumersPrint().ToList().Find(x => x.Id == parcely.SenderParcelToCostumer.id ) ;
            IDAL.DO.Costumer Getter = data.CostumersPrint().ToList().Find(x => x.Id == parcely.GetterParcelToCostumer.id)  ;
            Location SenderLCT = new Location(Sender.Longitude, Sender.Lattitude);
            Location GetterLCT = new Location(Getter.Longitude, Getter.Lattitude);
            return new Drone()
            {
                Id = drone.Id,
                Weight = (WeightCategories)drone.MaxWeigth,
                Model = drone.Modle,
                ParcelTransfer =( parcely is null ? null  :  new ParcelInTransfer()
                {
                    Id = parcely.Id , 
                    Pickup = SenderLCT   , 
                    Dst =   GetterLCT,
                    Distance =calculateDistance(SenderLCT , GetterLCT ) , 
                    Weight = parcely.Weight , 
                    Priorety = parcely.Priority , 
                    Sender = new ParcelToCostumer() { id = parcely.SenderParcelToCostumer.id, name = parcely.SenderParcelToCostumer.name } , 
                    Target = new ParcelToCostumer() { id = parcely.GetterParcelToCostumer.id, name = parcely.GetterParcelToCostumer.name }




                } ) 
            };
        }

        private BaseStation StationC(IDAL.DO.Station station) => new BaseStation(){
                Id = station.Id,
                NumOfFreeOnes = station.ChargeSlots,
                LoctConstant = new Location(station.Longitude, station.Lattitude),
                Name = station.Name , 
                DroneInChargeList = (from drones in data.DronesChargesPrint() where (drones.StaionId ==station.Id) select (new DroneInCharge() { 
                    id = drones.DroneId ,
                    BatteryStat = PullDataDrone(drones.DroneId).BatteryStat }  ) ).ToList ()  
                };


        private Costumer CostumerC(IDAL.DO.Costumer costumer) => new Costumer() { 
            Id = costumer.Id, 
            Loct = new Location(costumer.Longitude, costumer.Lattitude), 
            Name = costumer.Name, 
            Phone_Num = costumer.Phone , 
            FromClient = ( from package in data.ParcelsPrint() where (package.SenderId == costumer.Id)  select (ParcelC(package)) ) .ToList() ,
            ToClient = (from package in data.ParcelsPrint() where (package.TargetId == costumer.Id) select (ParcelC(package))).ToList()
        };


        private Parcel ParcelC(IDAL.DO.Parcel parcel)
        {
            List<IDAL.DO.Costumer> NameFinder = data.CostumersPrint().ToList(); 
            return new Parcel()
            {
                Id = parcel.Id,
                ParcelDelivered = parcel.Delivered,
                ParcelPickedUp = parcel.PickedUp,
                ParcelCreation = parcel.Requested,
                ParcelBinded = parcel.Schedulded,
                Priority = (Priorities)parcel.Priority,
                Weight = (WeightCategories)parcel.Weight,
                SenderParcelToCostumer = new ParcelToCostumer() { id = parcel.SenderId, name =NameFinder.Find(x => x.Id == parcel.SenderId ).Name  },
                GetterParcelToCostumer = new ParcelToCostumer() { id = parcel.TargetId ,  name = NameFinder.Find(x => x.Id == parcel.TargetId).Name }
            };
        }
    }
}

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
        public Drone GetDrone(int id)
        {
            return Convert(GetDroneToList(id));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone, int stationId)
        {
            // throw error if the DroneStat and Weight are out of the enum range
            IsInEnum(drone.DroneStat);
            IsInEnum(drone.Weight);
            DO.Drone DroneTmp = new DO.Drone() {
                Id = drone.Id,
                MaxWeigth = (DO.WeightCategories)drone.Weight,
                Modle = drone.Model
            };
            DO.Station PulledStaion;
            try
            {
                PulledStaion = data.GetStation(stationId);
                PulledStaion.ChargeSlots -= 1;
                drone.Current = new Location(PulledStaion.Longitude, PulledStaion.Lattitude);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
            try
            {
                data.AddDrone(DroneTmp);
                data.AddDroneCharge(new DO.DroneCharge { StaionId = stationId, DroneId = drone.Id });
                data.UpdateStations(PulledStaion);
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err);
            }
            drone.BatteryStat = RandomGen.NextDouble() * 20 + 20;
            drone.DroneStat = DroneStatuses.Matance;

            DroneList TmpDrnLst = new DroneList()
            {
                Battery = drone.BatteryStat,
                Id = drone.Id,
                Loct = drone.Current,
                DroneStat = drone.DroneStat,
                Model = drone.Model,
                Weight = drone.Weight
            };
            // if the data.AddDrone didnt fail its safe to add the drone
            drones.Add(TmpDrnLst);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneCharge(int droneId)
        {
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Free)
                throw new EnumNotInRightStatus<DroneStatuses>("drone is not not Free  , it is :  ", drony.DroneStat);

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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneReleaseCharge(int droneId, double chargingPeriod)
        {
            if (chargingPeriod < 0)
                throw new NotValidTimePeriod("you tried to enter time that is smaller than 0 (forbidden) ",chargingPeriod); 
            
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Matance)
                throw new EnumNotInRightStatus<DroneStatuses>("drone is not in matance , it is: ", drony.DroneStat);

            int stationId;
            try
            {
                stationId = data.GetDroneCharge(droneId).StaionId;
                ///time perios is in hours 
                drony.Battery = Math.Min(drony.Battery + ChargingSpeed * chargingPeriod, 100);
                drony.DroneStat = DroneStatuses.Free;
                data.DeleteDroneCharge(drony.Id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
            try
            {
                DO.Station station = data.GetStation(stationId);
                station.ChargeSlots += 1;
                data.UpdateStations(station);
            }
            // if the Station cant be found no worries maybe the station was deleted in the mean while
            catch (DO.IdDosntExists) { }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int droneId, string droneName)
        {
            DO.Drone Drony = new DO.Drone() {
                Id = droneId,
                Modle = droneName,
                MaxWeigth = data.GetDrone(droneId).MaxWeigth
            };
            try
            {
                data.UpdateDrones(Drony);
                // if the data.UpdateDrones didnt fail its safe to update our drone list
                GetDroneToList(droneId).Model = droneName;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void BindParcelToDrone(int droneId)
        {
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Free)
            {
                throw new EnumNotInRightStatus<DroneStatuses>("the dorne is not free", drony.DroneStat);
            }
            IEnumerable<DO.Parcel> parcels = data.GetParcels(x => ParcelStatusC(x) == ParcelStatus.Declared && x.Weight <= (DO.WeightCategories)drony.Weight && canreach(drony, x, getParcelLoctSender));
            DO.Parcel resParcel;
            try
            {
                resParcel = parcels.First();
            }
            catch (InvalidOperationException)
            {
                throw new CouldntFindPatcelThatsFits("There arent any free parcels (that fits the drone weight and location) to bind to the drone");
            }
            foreach (var pack in parcels)
            {
                if (pack.Priority > resParcel.Priority)
                    resParcel = pack;
                else if (pack.Priority == resParcel.Priority)
                {
                    if (pack.Weight > resParcel.Weight)
                        resParcel = pack;
                    else if (pack.Weight == resParcel.Weight)
                    {
                        if (calculateDistance(drony.Loct, getParcelLoctSender(pack)) < calculateDistance(drony.Loct, getParcelLoctSender(resParcel)))
                            resParcel = pack;
                    }
                }
            }
            drony.ParcelId = resParcel.Id;
            resParcel.Requested = DateTime.Now;
            resParcel.DroneId = drony.Id;
            drony.DroneStat = DroneStatuses.Delivery;
            try
            {
                data.UpdateParcles(resParcel);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DronePickUp(int droneId)
        {
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Delivery)
            {
                throw new EnumNotInRightStatus<DroneStatuses>("the Drone Should be IN delivery mode , it is in: ", drony.DroneStat);
            }
            DO.Parcel pack = data.GetParcel((int)drony.ParcelId);
            if (ParcelStatusC(pack) != ParcelStatus.Binded)
            {
                throw new EnumNotInRightStatus<ParcelStatus>("Parcel should be Binded when it is : ", ParcelStatusC(pack));
            }
            if (!canreach(drony, pack, getParcelLoctSender))
                throw new CantReachToDest("cant reach to the sender to pick up the parcel ", drony.Battery, getPowerUsage(drony.Loct, getParcelLoctSender(pack), (WeightCategories)pack.Weight));
            ///battery status changed !!! 
            drony.Battery -= getPowerUsage(getParcelLoctSender(pack), drony.Loct, (WeightCategories)pack.Weight);
            drony.Loct = getParcelLoctSender(pack);
            pack.PickedUp = DateTime.Now;
            try
            {
                data.UpdateParcles(pack);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneDelivere(int droneId)
        {
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Delivery)
            {
                throw new EnumNotInRightStatus<DroneStatuses>("Drone Should be in delivry!! it is now in the status of: ", drony.DroneStat);
            }
            DO.Parcel pack = data.GetParcel((int)drony.ParcelId);
            if (ParcelStatusC(pack) != ParcelStatus.PickedUp)
            {
                throw new EnumNotInRightStatus<ParcelStatus>("parcel is not in the status pickedup it is in : ", ParcelStatusC(pack));
            }
            Location Target = getParcelLoctTarget(pack);
            if (!canreach(drony, pack, getParcelLoctTarget))
            {
                throw new CantReachToDest("cant reach to the sender to pick up the parcel ", drony.Battery, getPowerUsage(drony.Loct, getParcelLoctSender(pack), (WeightCategories)pack.Weight));
            }
            drony.Battery -= getPowerUsage(Target, drony.Loct, (WeightCategories)pack.Weight);
            drony.Loct = new Location(Target.Longitude, Target.Lattitude);
            drony.DroneStat = DroneStatuses.Free;
            drony.ParcelId = null;
            pack.Delivered = DateTime.Now;
            try
            {
                data.UpdateParcles(pack);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            try
            {
                DroneList drone = GetDroneToList(id);
                if (drone.DroneStat != DroneStatuses.Free)
                    throw new CantDelete("cant delete the drone becouse he is not free", id);
                
                drones.Remove(drone);
                data.DeleteDrone(id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneList> GetDrones()
        {
            return drones;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneList> GetDronesFiltered(IEnumerable<DroneStatuses> statuses, IEnumerable<WeightCategories> weights)
        {
            return drones.Where(x => statuses.Contains(x.DroneStat) && weights.Contains(x.Weight));
        }
    }
}

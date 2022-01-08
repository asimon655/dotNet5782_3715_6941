using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public void AddParcel(Parcel parcel)
        {
            isInEnum<WeightCategories>(parcel.Weight);
            isInEnum<Priorities>(parcel.Priority); 
            
            try
            {
                data.GetCustomer(parcel.SenderParcelToCostumer.id);
                data.GetCustomer(parcel.GetterParcelToCostumer.id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
            if (parcel.SenderParcelToCostumer.id == parcel.GetterParcelToCostumer.id)
            {
                throw new SenderGetterAreSame("the sender and getter cant be the same ", parcel.GetterParcelToCostumer.id);
            }
            isInEnum(parcel.Priority);
            isInEnum(parcel.Weight);

            DO.Parcel ParcelTmp = new DO.Parcel() {
                Schedulded= DateTime.Now,
                Requested= null,
                PickedUp= null,
                Delivered= null,
                DroneId=null,                 
                SenderId=parcel.SenderParcelToCostumer.id,
                TargetId=parcel.GetterParcelToCostumer.id ,
                Priority=(DO.Priorities)parcel.Priority,
                Weight=(DO.WeightCategories)parcel.Weight
            };
            data.AddParcel(ParcelTmp);
        }
        public Parcel GetParcel(int id)
        {
            try
            {
                return Convert(data.GetParcel(id));
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            } 
        }

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
                else if(pack.Priority == resParcel.Priority)
                {
                    if (pack.Weight > resParcel.Weight)
                        resParcel = pack;
                    else if(pack.Weight == resParcel.Weight)
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
            catch(DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }

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
            drony.Battery -= getPowerUsage(getParcelLoctSender(pack), drony.Loct ,  (WeightCategories)pack.Weight);
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

        public void DroneDelivere(int droneId)
        {
            DroneList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Delivery)
                throw new  EnumNotInRightStatus<DroneStatuses>("Drone Should be in delivry!! it is now in the status of: ", drony.DroneStat);
            DO.Parcel pack = data.GetParcel((int)drony.ParcelId);
            if (ParcelStatusC(pack) != ParcelStatus.PickedUp)
            {
                throw new EnumNotInRightStatus<ParcelStatus>("parcel is not in the status pickedup it is in : ", ParcelStatusC(pack)); 
            }
            Location Target = getParcelLoctTarget(pack);
            if (!canreach(drony, pack, getParcelLoctTarget))
                throw new CantReachToDest("cant reach to the sender to pick up the parcel ", drony.Battery, getPowerUsage(drony.Loct, getParcelLoctSender(pack), (WeightCategories)pack.Weight));
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

        public IEnumerable<ParcelList> GetParcels()
        {
            try
            {
                List<ParcelList> tmpy = new List<ParcelList>();
                foreach (var x in data.GetParcels())
                    tmpy.Add(ConvertList(x));
                return tmpy;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }


        public IEnumerable<ParcelList> GetUnbindedParcels()
        {
            try
            {
                List<ParcelList> tmpy = new List<ParcelList>();
                // if the parcel.DroneId is null then the parcel is unbinded
                foreach (var x in data.GetParcels(x => x.DroneId is null && ParcelStatusC(x) == ParcelStatus.Declared))
                    tmpy.Add(new ParcelList()
                    {
                        Id = x.Id
                        ,
                        ParcelStatus = ParcelStatusC(x)
                        ,
                        Priorety = (Priorities)x.Priority
                        ,
                        SenderName = data.GetCustomer(x.SenderId).Name
                        ,
                        TargetName = data.GetCustomer(x.TargetId).Name
                        ,
                        Weight = (WeightCategories)x.Weight
                    });


                return tmpy;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }

        public void DeleteParcel(int id)
        {
            try
            {
                DO.Parcel parcel = data.GetParcel(id);
                if (parcel.DroneId is not null)
                    throw new CantDelete("can't delete the parcel because it has been bonded to a drone already", id);

                data.DeleteParcel(id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }

        public IEnumerable<BO.ParcelList> GetParcelsFiltered(IEnumerable<BO.WeightCategories> weights, IEnumerable<BO.Priorities> priorties,
                                                        DateTime? CreationFrom, DateTime? CreationTo,
                                                        DateTime? BindFrom, DateTime? BindTo,
                                                        DateTime? PickUpFrom, DateTime? PickUpTo,
                                                        DateTime? DeliverFrom, DateTime? DeliverTo)
        {
            return data.GetParcels(x => weights.Contains((BO.WeightCategories)x.Weight) && priorties.Contains((BO.Priorities)x.Priority) &&
                                    (CreationFrom is null || (x.Requested is not null && x.Requested >= CreationFrom)) &&
                                    (CreationTo is null || (x.Requested is not null && x.Requested <= CreationTo)) &&
                                    (BindFrom is null || (x.Schedulded is not null && x.Schedulded >= BindFrom)) &&
                                    (BindTo is null || (x.Schedulded is not null && x.Schedulded <= BindTo)) &&
                                    (PickUpFrom is null || (x.PickedUp is not null && x.PickedUp >= PickUpFrom)) &&
                                    (PickUpTo is null || (x.PickedUp is not null && x.PickedUp <= PickUpTo)) &&
                                    (DeliverFrom is null || (x.Delivered is not null && x.Delivered >= DeliverFrom)) &&
                                    (DeliverTo is null || (x.Delivered is not null && x.Delivered <= DeliverTo)))
                    .Select(ConvertList);
        }
    }
}
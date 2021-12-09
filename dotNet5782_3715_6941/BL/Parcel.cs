using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IBL
{
    namespace BO
    {
        public class Parcel 
        {
            public int Id { set; get; }
            public IBL.BO.ParcelToCostumer SenderParcelToCostumer { set; get; }
            public IBL.BO.ParcelToCostumer GetterParcelToCostumer { set; get; }

            public WeightCategories Weight {set; get ; }
            public Priorities Priority { set; get;  } 
            public ParcelToDrone? ParcelDrone { set; get;  } = null;
            public DateTime? ParcelCreation { set; get;  } = null;
            public DateTime? ParcelBinded { set; get;  } = null;
            public DateTime? ParcelPickedUp { set; get; } = null;
            public DateTime? ParcelDelivered { set; get;  } = null;

            public override string ToString()
            {
                return $"Id : {Id}\n" +
                       $"sender : {SenderParcelToCostumer}\n" +
                       $"getter : {GetterParcelToCostumer}\n" +
                       $"Weight : {Weight}\n" +
                       $"Priority : {Priority}\n" +
                       $"Priority : {(ParcelCreation is null ? ' ' : ParcelCreation)}\n" +
                       $"Priority : {(ParcelBinded is null ? ' ' : ParcelBinded)}\n" +
                       $"Priority : {(ParcelPickedUp is null ? ' ' : ParcelPickedUp)}\n" +
                       $"Priority : {(ParcelDelivered is null ? ' ' : ParcelDelivered)}\n" +
                       $"binded drone : {ParcelDrone}";
            }
        }
    } 
}
namespace BL
{
    public partial class Bl : IBL.Ibl
    {
        public void AddParcel(Parcel parcel)
        {
            isInEnum<WeightCategories>(parcel.Weight);
            isInEnum<Priorities>(parcel.Priority); 
            
            try
            {
                data.PullDataCostumer(parcel.SenderParcelToCostumer.id);
                data.PullDataCostumer(parcel.GetterParcelToCostumer.id);
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
                Requested= DateTime.Now,
                Schedulded= null,
                PickedUp= null,
                Delivered= null,
                DroneId=null,                 
                SenderId=parcel.SenderParcelToCostumer.id,
                TargetId=parcel.GetterParcelToCostumer.id ,
                Priority=(DO.Priorities)parcel.Priority,
                Weight=(DO.WeightCategories)parcel.Weight};
            data.AddParcel(ParcelTmp);
        }
        public Parcel PullDataParcel(int id)
        {
            try
            {
                return ParcelC(data.PullDataParcel(id));
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            } 
        }

        public void BindParcelToDrone(int droneId)
        {

            DroneToList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Free)
            {
                throw new EnumNotInRightStatus<DroneStatuses>("the dorne is not free", drony.DroneStat);
            }
            IEnumerable<DO.Parcel> parcels = data.GetParcels(x => ParcelStatC(x) == ParcelStat.Declared && canreach(drony, x, getParcelLoctSender));
            DO.Parcel resParcel;
            try
            {
                resParcel = parcels.First();
            }
            catch (InvalidOperationException)
            {
                throw new ThereArentAnyParcels("There arent any parcels to bind to drone");
            }
            foreach (var pack in parcels)
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
            if ((WeightCategories)resParcel.Weight > drony.Weight)
                throw new CouldntFindRightParcelWeight("douldnt find parcel in the weight of the drone or under ", drony.Weight, (WeightCategories)resParcel.Weight);
            drony.ParcelIdTransfer = resParcel.Id;
            resParcel.Schedulded = DateTime.Now;
            drony.DroneStat = DroneStatuses.Delivery;
            try
            {
                data.UpdateParcles(resParcel);
            }
            catch(DO.IdDosntExists err) {
                throw new IdDosntExists(err); 
                
            } 
            
            

        }

        public void PickUpByDrone(int droneId)
        {
            DroneToList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Delivery)
            {
                throw new EnumNotInRightStatus<DroneStatuses>("the Drone Should be IN delivery mode , it is in: ", drony.DroneStat);
            }
            DO.Parcel pack = data.PullDataParcel((int)drony.ParcelIdTransfer);
            if (ParcelStatC(pack) != ParcelStat.Binded)
            {
                throw new EnumNotInRightStatus<ParcelStat>("Parcel should be Binded when it is : ", ParcelStatC(pack)); 
            }
            if (!canreach(drony, pack, getParcelLoctSender))
                throw new CantReachToDest("cant reach to the sender to pick up the parcel ", drony.BatteryStat, getPowerUsage(drony.Current, getParcelLoctSender(pack), (WeightCategories)pack.Weight));
            ///battery status changed !!! 
            drony.BatteryStat -= getPowerUsage(getParcelLoctSender(pack), drony.Current ,  (WeightCategories)pack.Weight);
            drony.Current = getParcelLoctSender(pack);
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

        public void ParcelDeliveredToCostumer(int droneId)
        {
            DroneToList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Delivery)
                throw new  EnumNotInRightStatus<DroneStatuses>("Drone Should be in delivry!! it is now in the status of: ", drony.DroneStat);
            DO.Parcel pack = data.PullDataParcel((int)drony.ParcelIdTransfer);
            if (ParcelStatC(pack) != ParcelStat.PickedUp)
            {
                throw new EnumNotInRightStatus<ParcelStat>("parcel is not in the status pickedup it is in : ", ParcelStatC(pack)); 
            }
            Location Target = getParcelLoctTarget(pack);
            if (!canreach(drony, pack, getParcelLoctTarget))
                throw new CantReachToDest("cant reach to the sender to pick up the parcel ", drony.BatteryStat, getPowerUsage(drony.Current, getParcelLoctSender(pack), (WeightCategories)pack.Weight));
            drony.BatteryStat -= getPowerUsage(Target, drony.Current, (WeightCategories)pack.Weight);
            drony.Current = new Location(Target.Longitude, Target.Lattitude);
            drony.DroneStat = DroneStatuses.Free;
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

        public IEnumerable<ParcelToList> ParcelsPrint()
        {
            try
            {
                List<ParcelToList> tmpy = new List<ParcelToList>();
                foreach (var x in data.ParcelsPrint())
                    tmpy.Add(new ParcelToList()
                    {
                        Id = x.Id
                        ,
                        ParcelStatus = ParcelStatC(x)
                        ,
                        Priorety = (Priorities)x.Priority
                        ,
                        SenderName = data.PullDataCostumer(x.SenderId).Name
                        ,
                        TargetName = data.PullDataCostumer(x.TargetId).Name
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
        public IEnumerable<ParcelToList> ParcelsWithoutDronesPrint()
        {
            try
            {
                List<ParcelToList> tmpy = new List<ParcelToList>();
                // if the parcel.DroneId is null then the parcel is unbinded
                foreach (var x in data.GetParcels(x => x.DroneId is null && ParcelStatC(x) == ParcelStat.Declared))
                    tmpy.Add(new ParcelToList()
                    {
                        Id = x.Id
                        ,
                        ParcelStatus = ParcelStatC(x)
                        ,
                        Priorety = (Priorities)x.Priority
                        ,
                        SenderName = data.PullDataCostumer(x.SenderId).Name
                        ,
                        TargetName = data.PullDataCostumer(x.TargetId).Name
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



    }
}
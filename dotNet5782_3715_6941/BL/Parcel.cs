﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IBL
{
    namespace BO
    {
        public class Parcel : Printable
        {
            public int Id { set; get; }
            public IBL.BO.ParcelToCostumer SenderParcelToCostumer { set; get; }
            public IBL.BO.ParcelToCostumer GetterParcelToCostumer { set; get; }

            public WeightCategories Weight {set; get ; }
            public Priorities Priority { set; get;  } 
            public ParcelToDrone ParcelDrone { set; get;  }
            public DateTime ParcelCreation { set; get;  } 
            public DateTime ParcelBinded { set; get;  }
            public DateTime ParcelPickedUp { set; get; }
            public DateTime ParcelDelivered { set; get;  }

        }
    } 
}
namespace BL
{
    public partial class Bl : IBL.Ibl
    {
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
        public void AddParcel(Parcel parcel)
        {
            isInEnum<WeightCategories>(parcel.Weight);
            isInEnum<Priorities>(parcel.Priority); 
            
            try
            {
                data.PullDataCostumer(parcel.SenderParcelToCostumer.id);
                data.PullDataCostumer(parcel.GetterParcelToCostumer.id);
            }
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
            if (parcel.SenderParcelToCostumer.id == parcel.GetterParcelToCostumer.id)
            {
                throw new SenderGetterAreSame("the sender and getter cant be the same ", parcel.GetterParcelToCostumer.id);
            }
            isInEnum(parcel.Priority);
            isInEnum(parcel.Weight);

            IDAL.DO.Parcel ParcelTmp = new IDAL.DO.Parcel() {
                Requested= DateTime.Now, 
                SenderId=parcel.SenderParcelToCostumer.id,
                TargetId=parcel.GetterParcelToCostumer.id ,
                Priority=(IDAL.DO.Priorities)parcel.Priority,
                Weight=(IDAL.DO.WeightCategories)parcel.Weight};
            data.AddParcel(ParcelTmp);
        }
        public Parcel PullDataParcel(int id)
        {
            try
            {
                return ParcelC(data.PullDataParcel(id));
            }
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            } 
        }

        public void BindParcelToDrone(int droneId)
        {

            IEnumerable<IDAL.DO.Parcel> list = data.ParcelsPrint();
            IDAL.DO.Parcel  resParcel = list.First();
            DroneToList drony = GetDroneToList(droneId);
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
            if ((WeightCategories)resParcel.Weight > drony.Weight)
                throw new CouldntFindRightParcel("douldnt find parcel in the weight of the drone or under ", drony.Weight, (WeightCategories)resParcel.Weight);
            drony.ParcelIdTransfer = resParcel.Id;
            resParcel.Schedulded = DateTime.Now;
            drony.DroneStat = DroneStatuses.Delivery;
            try
            {
                data.UpdateParcles(resParcel);
            }
            catch(IDAL.DO.IdDosntExists err) {
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
            IDAL.DO.Parcel pack = data.PullDataParcel(drony.ParcelIdTransfer);
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
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            } 
        }

        public void ParcelDeliveredToCostumer(int droneId)
        {
            DroneToList drony = GetDroneToList(droneId);
            IDAL.DO.Parcel pack = data.PullDataParcel(drony.ParcelIdTransfer);
            Location Target = getParcelLoctTarget(pack);
            if (ParcelStatC(pack) != ParcelStat.PickedUp)
            {
                throw new EnumNotInRightStatus<ParcelStat>("parcel is not in the status pickedup it is in : ", ParcelStatC(pack)); 
            }
            if (!canreach(drony, pack, getParcelLoctTarget))
                throw new CantReachToDest("cant reach to the sender to pick up the parcel ", drony.BatteryStat, getPowerUsage(drony.Current, getParcelLoctSender(pack), (WeightCategories)pack.Weight));
            drony.BatteryStat -= getPowerUsage(Target, drony.Current, (WeightCategories)pack.Weight);
            drony.Current = Target;
            drony.DroneStat = DroneStatuses.Free;
            pack.Delivered = DateTime.Now;
            try
            {
                data.UpdateParcles(pack);
            }
            catch (IDAL.DO.IdDosntExists err)
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
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        public IEnumerable<ParcelToList> ParcelsWithoutDronesPrint()
        {
            try
            {
                List<ParcelToList> tmpy = new List<ParcelToList>();
                foreach (var x in data.ParcelWithoutDronePrint())
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
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }



    }
}
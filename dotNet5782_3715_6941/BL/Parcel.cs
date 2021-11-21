using IBL.BO;
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
            IDAL.DO.Parcel ParcelTmp = new IDAL.DO.Parcel() { Id = parcel.Id,
                Requested= DateTime.Now, 
                SenderId=parcel.SenderParcelToCostumer.id,
                TargetId=parcel.GetterParcelToCostumer.id ,
                Priority=(IDAL.DO.Priorities)parcel.Priority,
                Weight=(IDAL.DO.WeightCategories)parcel.Weight};
            data.AddParcel(ParcelTmp);
        }
        public Parcel PullDataParcel(int id)
        {
            return ParcelC(data.PullDataParcel(id));
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
            drony.ParcelIdTransfer = resParcel.Id;
            resParcel.Schedulded = DateTime.Now;
            drony.DroneStat = DroneStatuses.Delivery;
            data.UpdateParcles(resParcel);
            
            

        }

        public void PickUpByDrone(int droneId)
        {
            DroneToList drony = GetDroneToList(droneId);
            if (drony.DroneStat != DroneStatuses.Delivery)
            {
                throw new NotImplementedException();
            }
            IDAL.DO.Parcel pack = data.PullDataParcel(drony.ParcelIdTransfer);
            if (ParcelStatC(pack) != ParcelStat.Binded)
            {
                throw new NotImplementedException();
            }
            if (!canreach(drony, pack,getParcelLoctSender))
                throw new NotImplementedException();
            ///battery status changed !!! 
            drony.BatteryStat -= getPowerUsage(getParcelLoctSender(pack), drony.Current ,  (WeightCategories)pack.Weight);
            drony.Current = getParcelLoctSender(pack);
            pack.PickedUp = DateTime.Now;
            data.UpdateParcles(pack);
        }

        public void ParcelDeliveredToCostumer(int droneId)
        {
            DroneToList drony = GetDroneToList(droneId);
            IDAL.DO.Parcel pack = data.PullDataParcel(drony.ParcelIdTransfer);
            Location Target = getParcelLoctTarget(pack);
            if (ParcelStatC(pack) != ParcelStat.PickedUp)
            {
                throw new NotImplementedException();
            }
            if (!canreach(drony, pack, getParcelLoctTarget))
                throw new NotImplementedException();
            drony.BatteryStat -= getPowerUsage(Target, drony.Current, (WeightCategories)pack.Weight);
            drony.Current = Target;
            drony.DroneStat = DroneStatuses.Free;
            pack.Delivered = DateTime.Now;
            data.UpdateParcles(pack);
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



    }
}
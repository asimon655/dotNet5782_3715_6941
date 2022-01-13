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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcel)
        {
            // throw error if the Weight and Priority are out of the enum range
            IsInEnum(parcel.Weight);
            IsInEnum(parcel.Priority);

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

            DO.Parcel ParcelTmp = new DO.Parcel()
            {
                Schedulded = DateTime.Now,
                Requested = null,
                PickedUp = null,
                Delivered = null,
                DroneId = null,
                SenderId = parcel.SenderParcelToCostumer.id,
                TargetId = parcel.GetterParcelToCostumer.id,
                Priority = (DO.Priorities)parcel.Priority,
                Weight = (DO.WeightCategories)parcel.Weight
            };
            try
            {
                data.AddParcel(ParcelTmp);
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            try
            {
                DO.Parcel parcel = data.GetParcel(id);
                if (parcel.DroneId is not null)
                {
                    throw new CantDelete("can't delete the parcel because it has been bonded to a drone already", id);
                }

                data.DeleteParcel(id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelList> GetParcels()
        {
            return data.GetParcels().Select(ConvertList);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelList> GetUnbindedParcels()
        {
            return data.GetParcels(x => x.DroneId is null && ParcelStatusC(x) == ParcelStatus.Declared).Select(ConvertList);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelList> GetParcelsFiltered(IEnumerable<BO.WeightCategories> weights, IEnumerable<BO.Priorities> priorties,
                                                        DateTime? CreationFrom, DateTime? CreationTo,
                                                        DateTime? BindFrom, DateTime? BindTo,
                                                        DateTime? PickUpFrom, DateTime? PickUpTo,
                                                        DateTime? DeliverFrom, DateTime? DeliverTo)
        {
            return (data.GetParcels(x => weights.Contains((BO.WeightCategories)x.Weight) && priorties.Contains((BO.Priorities)x.Priority) &&
                                   (CreationFrom is null || (x.Requested is not null && x.Requested >= CreationFrom)) &&
                                   (CreationTo is null || (x.Requested is not null && x.Requested <= CreationTo)) &&
                                   (BindFrom is null || (x.Schedulded is not null && x.Schedulded >= BindFrom)) &&
                                   (BindTo is null || (x.Schedulded is not null && x.Schedulded <= BindTo)) &&
                                   (PickUpFrom is null || (x.PickedUp is not null && x.PickedUp >= PickUpFrom)) &&
                                   (PickUpTo is null || (x.PickedUp is not null && x.PickedUp <= PickUpTo)) &&
                                   (DeliverFrom is null || (x.Delivered is not null && x.Delivered >= DeliverFrom)) &&
                                   (DeliverTo is null || (x.Delivered is not null && x.Delivered <= DeliverTo)))
                    ).Select(ConvertList);
        }
    }
}
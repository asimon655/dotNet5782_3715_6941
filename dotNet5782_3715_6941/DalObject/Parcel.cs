using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcel)
        {
            parcel.IsDeleted = false;
            parcel.Id = ++DataSource.Config.IdCreation;
            DataSource.Parcels.Add(parcel);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            Parcel parcel = DataSource.Parcels.Find(s => !s.IsDeleted && s.Id == id);
            /// if the Parcel wasnt found throw error
            if (parcel.Id != id)
            {
                throw new IdDosntExists("the Id could not be found", id);
            }
            return parcel;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels()
        {
            return DataSource.Parcels.FindAll(s => !s.IsDeleted);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcles(Parcel parcel)
        {
            // if we cant find that the id we throw error
            if (Update(DataSource.Parcels, parcel) == -1)
            {
                throw new IdDosntExists("the Id Parcel is dosnt exists", parcel.Id);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            // if we cant find that the id we throw error
            if (Delete(DataSource.Parcels, id) == -1)
            {
                throw new IdDosntExists("the Id Parcel is dosnt exists", id);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr)
        {
            return DataSource.Parcels.FindAll(s => !s.IsDeleted && expr(s));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CountParcels(Func<Parcel, bool> expr)
        {
            return DataSource.Parcels.Count(s => !s.IsDeleted && expr(s));
        }
    }
}

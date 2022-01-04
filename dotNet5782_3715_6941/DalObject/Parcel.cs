using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        public void AddParcel(Parcel parcel)
        {
            parcel.IsDeleted = false;
            parcel.Id = ++DataSource.Config.IdCreation;
            DataSource.Parcels.Add(parcel);
        }
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
        public IEnumerable<Parcel> GetParcels()
        {
            return DataSource.Parcels.FindAll(s => !s.IsDeleted);
        }
        public void UpdateParcles(Parcel parcel)
        {
            // if we cant find that the id we throw error
            if (Update(DataSource.Parcels, parcel) == -1)
            {
                throw new IdDosntExists("the Id Parcel is dosnt exists", parcel.Id);
            }
        }
        public void DeleteParcel(int id)
        {
            // if we cant find that the id we throw error
            if (Delete(DataSource.Parcels, id) == -1)
            {
                throw new IdDosntExists("the Id Parcel is dosnt exists", id);
            }
        }
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr)
        {
            return DataSource.Parcels.FindAll(s => !s.IsDeleted && expr(s));
        }
        public int CountParcels(Func<Parcel, bool> expr)
        {
            return DataSource.Parcels.Count(s => !s.IsDeleted && expr(s));
        }
    }
}

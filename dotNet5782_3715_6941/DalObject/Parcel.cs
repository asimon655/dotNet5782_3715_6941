using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace DalObject
{
    public partial class DalObject : DalApi.IDal
    {
        public void AddParcel(Parcel parcel)
        {
            parcel.Id = ++DataSource.Config.IdCreation;
            DataSource.Parcels.Add(parcel);
        }
        public Parcel PullDataParcel(int id)
        {
            Parcel parcel = DataSource.Parcels.Find(s => s.Id == id);
            /// if the Parcel wasnt found throw error
            if (parcel.Id != id)
            {
                throw new IdDosntExists("the Id could not be found", id);
            }
            return parcel;
        }
        public IEnumerable<Parcel> ParcelsPrint()
        {
            return DataSource.Parcels;
        }
        public void UpdateParcles(Parcel parcel)
        {
            // if we cant find that the id we throw error
            if (!DataSource.Parcels.Any(s => s.Id == parcel.Id))
            {
                throw new IdDosntExists("the Id Parcel is dosnt exists", parcel.Id);
            }
            Update(DataSource.Parcels, parcel);
        }
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr)
        {
            return DataSource.Parcels.FindAll(expr);
        }
        public int CountParcels(Func<Parcel, bool> expr)
        {
            return DataSource.Parcels.Count(expr);
        }
    }
}

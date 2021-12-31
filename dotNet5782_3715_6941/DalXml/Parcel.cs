using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        
        public void AddParcel(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcles(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        public int CountParcels(Func<Parcel, bool> expr)
        {
            throw new NotImplementedException();
        }
    }
}

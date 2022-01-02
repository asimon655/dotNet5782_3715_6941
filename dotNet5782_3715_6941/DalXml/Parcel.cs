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
            List<Parcel> parcels = Read<Parcel>();

            parcel.Id = XmlConfig.GetPromoteParcelIndex();

            parcels.Add(parcel);

            Write(parcels);
        }

        public Parcel GetParcel(int id)
        {
            List<Parcel> parcels = Read<Parcel>();

            Parcel parcel = parcels.Find(s => s.Id == id);

            /// if the Customer wasnt found throw error
            if (parcel.Id != id)
            {
                throw new IdDosntExists("the Id could not be found", id);
            }
            return parcel;
        }

        public IEnumerable<Parcel> GetParcels()
        {
            return Read<Parcel>();
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> expr)
        {
            return Read<Parcel>().FindAll(expr);
        }

        public int CountParcels(Func<Parcel, bool> expr)
        {
            return Read<Parcel>().Count(expr);
        }

        public void UpdateParcles(Parcel parcel)
        {
            List<Parcel> parcels = Read<Parcel>();

            if (Update(parcels, parcel) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", parcel.Id);
            }

            Write(parcels);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
/*
_________ ______            _______
\__   __/(  __  \ |\     /|(       )
   ) (   | (  \  )| )   ( || () () |
   | |   | |   ) || (___) || || || |
   | |   | |   | ||  ___  || |(_)| |
   | |   | |   ) || (   ) || |   | |
___) (___| (__/  )| )   ( || )   ( |
\_______/(______/ |/     \||/     \|



 
 _______  _______ _________ _______  _______  _
(  ___  )(  ____ \\__   __/(       )(  ___  )( (    /|
| (   ) || (    \/   ) (   | () () || (   ) ||  \  ( |
| (___) || (_____    | |   | || || || |   | ||   \ | |
|  ___  |(_____  )   | |   | |(_)| || |   | || (\ \) |
| (   ) |      ) |   | |   | |   | || |   | || | \   |
| )   ( |/\____) |___) (___| )   ( || (___) || )  \  |
|/     \|\_______)\_______/|/     \|(_______)|/    )_)


 
 */
namespace IDAL
{

    namespace DO
    {
        public struct Parcel
        {
            public int Id { set; get; }
            public int SenderId { set; get; }
            public int TargetId { set; get; }
            public WeightCategories Weight { set; get; }
            public Priorities Priority { set; get; }
            public int? DroneId { set; get; }
            public DateTime? Requested { set; get; }
            public DateTime? Schedulded { set; get; }
            public DateTime? PickedUp { set; get; }
            public DateTime? Delivered { set; get; }

        }
    }
}
namespace DalObject
{
    public partial class DalObject : IDAL.Idal
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
        public IEnumerable<Parcel> ParcelWithoutDronePrint()
        {
            return DataSource.Parcels.FindAll(x => x.Schedulded == DateTime.MinValue);
            
        }
    }
}

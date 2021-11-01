using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            public DateTime Requested { set; get; }
            public int DroneId { set; get; }
            public DateTime Schedulded { set; get; }
            public DateTime PickedUp { set; get; }
            public DateTime Delivered { set; get; }

            public override string ToString()
            {
                return "Id: " + Id.ToString() + " SenderId: " + SenderId.ToString() + " TargetId: " + TargetId.ToString() + " Weight: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>(this.Weight) + " Priorty: " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Priorities>(this.Priority) + " Requested: " + Requested.ToString() + " Sechuled: " + Schedulded.ToString() + " PickedUp: " + this.PickedUp.ToString() + " Delivered " + Delivered.ToString() + " DroneId: " + DroneId.ToString(); /// returns strings with all the args of the struct in string and longitude and lattiude in Sexagesimal show 
            }
        }
    }
}
namespace DAL
{
    namespace DalObject : IDAL.Idal
    {
        partial class DalObject
        {
            public void AddParcel(Parcel parcel)
            {
                parcel.Id = ++DataSource.Config.IdCreation;
                DataSource.Parcels.Add(parcel);
            }
            public Parcel? PullDataParcel(int id)
            {
                Parcel parcel = DataSource.Parcels.Find(s => s.Id == id);
                /// if the Parcel wasnt found return null
                if (parcel.Id != id)
                    return null;
                return parcel;
            }
            public IEnumrable<Parcel> ParcelsPrint()
            {
                return DAL.DalObject.DataSource.Parcels;
            }
        }
    }
}
using System;

namespace BO
{
    public class Parcel 
    {
        public int Id { set; get; }
        public CustomerInParcel SenderParcelToCostumer { set; get; }
        public CustomerInParcel GetterParcelToCostumer { set; get; }

        public WeightCategories Weight {set; get ; }
        public Priorities Priority { set; get;  } 
        public DroneInParcel? ParcelDrone { set; get;  } = null;
        public DateTime? ParcelCreation { set; get;  } = null;
        public DateTime? ParcelBinded { set; get;  } = null;
        public DateTime? ParcelPickedUp { set; get; } = null;
        public DateTime? ParcelDelivered { set; get;  } = null;

        public override string ToString()
        {
           
            return $"Id : {Id}\n" +
                    $"sender : {SenderParcelToCostumer}\n" +
                    $"getter : {GetterParcelToCostumer}\n" +
                    $"Weight : {Weight}\n" +
                    $"Priority : {Priority}\n" +
                    $"Priority : {(ParcelCreation is null ? ' ' : ParcelCreation)}\n" +
                    $"Priority : {(ParcelBinded is null ? ' ' : ParcelBinded)}\n" +
                    $"Priority : {(ParcelPickedUp is null ? ' ' : ParcelPickedUp)}\n" +
                    $"Priority : {(ParcelDelivered is null ? ' ' : ParcelDelivered)}\n" +
                    $"binded drone : {ParcelDrone}";
        }
    }
}

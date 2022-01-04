using System;

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
        public bool IsDeleted { set; get; }

    }
}
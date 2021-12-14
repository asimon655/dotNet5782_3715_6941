﻿namespace BO
{
    public class ClientToList 
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Phone { set; get; }
        public int ParcelDeliveredAndGot { set; get; }
        public int ParcelDeliveredAndNotGot { set; get; }
        public int ParcelGot { set; get; }
        public int InTheWay { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Name : {Name}\n" +
                    $"phone : {Phone}\n" +
                    $"parcel he sent on the way : {ParcelDeliveredAndNotGot}\n" +
                    $"parcel he sent that got delivered : {ParcelDeliveredAndGot}\n" +
                    $"parcel sent to him on the way : {InTheWay}\n" +
                    $"parcel sent to him that got delivered : {ParcelGot}";
        }
    }
}

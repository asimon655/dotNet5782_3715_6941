﻿namespace BO
{
    public class ParcelInDrone
    {
        public int Id { set; get; }
        public Priorities Priorety { set; get; }
        public WeightCategories Weight { set; get; }
        public CustomerInParcel Sender { set; get; }
        public CustomerInParcel Target { set; get; }
        public Location Pickup { set; get; }
        public Location Dst { set; get; }
        public double Distance { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Priorety : {Priorety}\n" +
                    $"Weight : {Weight}\n" +
                    $"Sender : {Sender}\n" +
                    $"Target : {Target}\n" +
                    $"Sender location : {Pickup}\n" +
                    $"Getter location : {Dst}\n" +
                    $"distance : {Distance}";
        }

    }
}

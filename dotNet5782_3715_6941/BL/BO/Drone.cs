﻿namespace BO
{
    public class Drone 
    {
        public int Id { set; get;  }
        public string Model { set; get; }
        public WeightCategories Weight { set; get; }
        public double BatteryStat { set; get;  }
        public DroneStatuses DroneStat { set; get;  }
        public ParcelInDrone? ParcelTransfer { set; get;  } = null;
        public Location Current { set; get;  }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Model : {Model}\n" +
                    $"location : {Current}\n" +
                    $"battary : {BatteryStat}\n" +
                    $"Max Weight : {Weight}\n" +
                    $"Drone Status : {DroneStat}\n" +
                    $"binded parcele : {ParcelTransfer}";
        }
    }
}

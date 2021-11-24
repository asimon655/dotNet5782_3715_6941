﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneToList 
        {
            public int Id { set; get; }
            public string Model { set; get; }
            public WeightCategories Weight { set; get; }
            public double BatteryStat { set; get; }
            public DroneStatuses DroneStat { set; get;  }
            public Location Current { set; get;  }
            public int ParcelIdTransfer { set; get; }

            public override string ToString()
            {
                return $"Id : {Id}\n" +
                       $"Model : {Model}\n" +
                       $"location : {Current}\n" +
                       $"battary : {BatteryStat}\n" +
                       $"Max Weight : {Weight}\n" +
                       $"Drone Status : {DroneStat}\n" +
                       $"binded parcele Id : {ParcelIdTransfer}";
            }
        }
    }
}

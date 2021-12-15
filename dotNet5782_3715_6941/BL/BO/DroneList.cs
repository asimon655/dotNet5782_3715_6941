namespace BO
{
    public class DroneList 
    {
        public int Id { set; get; }
        public string Model { set; get; }
        public WeightCategories Weight { set; get; }
        public double Battery { set; get; }
        public DroneStatuses DroneStat { set; get;  }
        public Location Loct { set; get;  }
        public int? ParcelId { set; get; } = null;

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Model : {Model}\n" +
                    $"location : {Loct}\n" +
                    $"battary : {Battery}\n" +
                    $"Max Weight : {Weight}\n" +
                    $"Drone Status : {DroneStat}\n" +
                    $"binded parcele Id : {ParcelId}";
        }
    }
}

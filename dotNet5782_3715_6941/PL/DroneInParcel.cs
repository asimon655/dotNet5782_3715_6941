namespace BO
{
    public class DroneInParcel
    {
        public int Id { set; get; }
        public double Battery { set; get; }
        public Location Loct { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"location : {Loct}\n" +
                    $"battary : {Battery}";
        }
    }
}

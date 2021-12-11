namespace BO
{
    public class ParcelToDrone 
    {
        public int Id { set; get; }
        public double BatteryStat { set; get; }
        public Location Loct { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"location : {Loct}\n" +
                    $"battary : {BatteryStat}";
        }
    }
}

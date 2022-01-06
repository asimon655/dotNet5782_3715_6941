namespace DO
{
    public struct Station
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public double Lattitude { set; get; }
        public double Longitude { set; get; }
        public int ChargeSlots { set; get; }
        public bool IsDeleted { set; get; }

    }

}
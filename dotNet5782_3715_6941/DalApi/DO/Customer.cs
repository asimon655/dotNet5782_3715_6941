namespace DO
{
    public struct Customer
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Phone { set; get; }
        public double Longitude { set; get; }
        public double Lattitude { set; get; }
        public bool IsDeleted { set; get; }
    }

}
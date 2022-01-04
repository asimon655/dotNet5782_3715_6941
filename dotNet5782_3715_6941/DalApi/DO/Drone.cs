namespace DO
{

    public struct Drone
    {
        public int Id { set; get; }
        public string Modle { set; get; }
        public WeightCategories MaxWeigth { set; get; }
        public bool IsDeleted { set; get; }
    }

}
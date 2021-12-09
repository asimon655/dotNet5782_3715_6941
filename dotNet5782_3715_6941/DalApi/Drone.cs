namespace DO
{

    public struct Drone
    {
        public int Id { set; get; }
        public string Modle { set; get; }
        public WeightCategories MaxWeigth { set; get; }


        public override string ToString()
        {
            return "ID: " + this.Id.ToString() + " Model: " + this.Modle.ToString() + " MaxWeight: " + this.MaxWeigth.ToString(); /// returns strings with all the args of the struct in string  
        }
    }

}
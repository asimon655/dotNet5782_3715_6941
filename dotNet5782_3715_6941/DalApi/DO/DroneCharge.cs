namespace DO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }
        public int StaionId { get; set; }

        public override string ToString()
        {
            return "DroneId: " + DroneId.ToString() + " StaionId: " + StaionId.ToString(); /// returns strings with all the args of the struct in string 
        }
    }
}

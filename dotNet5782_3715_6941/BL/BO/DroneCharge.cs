namespace BO
{
    public class DroneCharge 
    {
        public int DroneId { set; get;  }
        public double Battery { set; get;  }

        public override string ToString()
        {
            return $"Id : {DroneId}\n" +
                    $"battery : {Battery}\n";
        }
    }
}

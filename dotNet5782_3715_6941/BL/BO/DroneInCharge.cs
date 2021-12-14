namespace BO
{
    public class DroneInCharge 
    {
        public int id { set; get;  }
        public double BatteryStat { set; get;  }

        public override string ToString()
        {
            return $"Id : {id}\n" +
                    $"battery : {BatteryStat}\n";
        }
    }
}

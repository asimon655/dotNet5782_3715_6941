namespace BO
{
    public class ParcelList 
    {
        public int Id { set; get;  }
        public string SenderName { set; get;  }
        public string TargetName { set; get;  }
        public WeightCategories Weight { set; get;  }
        public Priorities Priorety { set; get;  }
        public ParcelStatus ParcelStatus { set; get;  }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"sender name : {SenderName}\n" +
                    $"getter name : {TargetName}\n" +
                    $"Weight : {Weight}\n" +
                    $"Priority : {Priorety}\n" +
                    $"status : {ParcelStatus}";
        }
    }
}

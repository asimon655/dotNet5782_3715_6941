
namespace BO
{
    public class BaseStaionToList 
    {
        public int Id { set; get; }
        public int Name { set; get; }
        public int NumOfNotFreeOne { set; get;  } 
        public int NumOfFreeOnes { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Name : {Name}\n" +
                    $"occupied charging slots : {NumOfNotFreeOne}\n" +
                    $"free charging slots : {NumOfFreeOnes}";
        }
    }
}

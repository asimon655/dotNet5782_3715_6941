using BO;

namespace BO
{
    public class CustomerToParcel 
    {
        public int Id { set; get; }
        public WeightCategories Weight {set; get ; }
        public Priorities Priority { set; get;  } 
        public ParcelStat Status { set; get; }
        public ParcelToCostumer ParentCustomer { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"\tWeight : {Weight}\n" +
                    $"\tPriority : {Priority}\n" +
                    $"\tStatus : {Status}\n" +
                    $"\tparent customer : \n{ParentCustomer}";
        }
    }

}
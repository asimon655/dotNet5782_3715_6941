namespace BO
{
    public class ParcelInCustomer
    {
        public int Id { set; get; }
        public WeightCategories Weight { set; get; }
        public Priorities Priority { set; get; }
        public ParcelStatus Status { set; get; }
        public CustomerInParcel ParentCustomer { set; get; }

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
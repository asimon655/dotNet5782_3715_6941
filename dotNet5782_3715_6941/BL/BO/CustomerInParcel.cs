namespace BO
{
    public class CustomerInParcel
    {
        public int id { set; get; }
        public string name { set; get; }


        public override string ToString()
        {
            return $"\t\tId : {id}\n" +
                    $"\t\tName : {name}";
        }
    }
}

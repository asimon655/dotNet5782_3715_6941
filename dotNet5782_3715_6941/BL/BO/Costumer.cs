using System.Collections.Generic;

namespace BO
{
    public class Costumer 
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone_Num { get; set; }
        public Location Loct { get; set; }
        public List<CustomerToParcel> FromClient { get; set; } 
        public List<CustomerToParcel> ToClient { get; set; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Name : {Name}\n" +
                    $"location : {Loct}\n" +
                    $"phone : {Phone_Num}\n" +
                    $"parceles sent to him : {string.Join('\n', ToClient)}\n" +
                    $"parceles he sent : {string.Join('\n', FromClient)}";
        }
    }
}

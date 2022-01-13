
namespace BO
{
    public class StationList
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int BusyPorts { set; get; }
        public int FreePorts { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Name : {Name}\n" +
                    $"occupied charging slots : {BusyPorts}\n" +
                    $"free charging slots : {FreePorts}";
        }
    }
}

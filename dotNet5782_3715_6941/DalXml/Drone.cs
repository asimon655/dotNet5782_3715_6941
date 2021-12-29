using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        
        public void AddDrone(Drone drone)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetDrones()
        {
            return new List<Drone>();
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> expr)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrones(Drone drone)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        public void AddStation(Station station)
        {
            List<Station> data = Read<Station>();

            if (data.Any(x => x.Id == station.Id))
                throw new IdAlreadyExists("there is already a station with that id", station.Id);

            data.Add(station);

            Write(data);
        }

        public Station GetStation(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetStations(Predicate<Station> expr)
        {
            throw new NotImplementedException();
        }

        public void UpdateStations(Station station)
        {
            throw new NotImplementedException();
        }
    }
}

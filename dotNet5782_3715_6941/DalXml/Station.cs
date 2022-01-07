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
            station.IsDeleted = false;

            List<Station> data = Read<Station>();

            if (data.Any(x => x.Id == station.Id))
                throw new IdAlreadyExists("there is already a station with that id", station.Id);

            data.Add(station);

            Write(data);
        }

        public Station GetStation(int id)
        {
            List<Station> stations = Read<Station>();

            Station station = stations.Find(s => !s.IsDeleted && s.Id == id);

            /// if the Station wasnt found throw error
            if (station.Id != id)
            {
                throw new IdDosntExists("the Id could not be found", id);
            }
            return station;
        }

        public IEnumerable<Station> GetStations()
        {
            return Read<Station>().FindAll(s => !s.IsDeleted);
        }

        public IEnumerable<Station> GetStations(Predicate<Station> expr)
        {
            return Read<Station>().FindAll(s => !s.IsDeleted && expr(s));
        }

        public int CountStations(Func<Station, bool> expr)
        {
            return Read<Station>().Count(s => !s.IsDeleted && expr(s));
        }
        
        public void UpdateStations(Station station)
        {
            List<Station> stations = Read<Station>();

            if (Update(stations, station) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", station.Id);
            }
            
            Write(stations);
        }
        public void DeleteStation(int id)
        {
            List<Station> stations = Read<Station>();

            if (Delete(stations, id) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", id);
            }

            Write(stations);
        }
    }
}

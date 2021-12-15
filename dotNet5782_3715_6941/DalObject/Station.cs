using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        public void AddStation(Station station)
        {
            if (DataSource.Stations.Any(s => s.Id == station.Id))
            {
                throw new IdAlreadyExists("the station Id is already taken", station.Id);
            }

            DataSource.Stations.Add(station);
        }
        public Station GetStation(int id)
        {
            Station station = DataSource.Stations.Find(s => s.Id == id);
            /// if the Station wasnt found throw error
            if (station.Id != id)
            {
                throw new IdDosntExists("the id could not be found", id);
            }
            return station;
        }
        public IEnumerable<Station> GetStations()
        {
            return DataSource.Stations;
        }
        public void UpdateStations(Station station)
        {
            /// if the Station wasnt found throw error
            if (!DataSource.Stations.Any(s => s.Id == station.Id))
            {
                throw new IdDosntExists("the id could not be found", station.Id);
            }

            Update(DataSource.Stations, station);
        }
        public IEnumerable<Station> GetStations(Predicate<Station> expr)
        {
            return DataSource.Stations.FindAll(expr);
        }
    }
}

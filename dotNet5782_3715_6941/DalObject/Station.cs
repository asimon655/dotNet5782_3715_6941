using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            station.IsDeleted = false;

            if (DataSource.Stations.Any(s => s.Id == station.Id))
            {
                throw new IdAlreadyExists("the station Id is already taken", station.Id);
            }

            DataSource.Stations.Add(station);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            Station station = DataSource.Stations.Find(s => !s.IsDeleted && s.Id == id);
            /// if the Station wasnt found throw error
            if (station.Id != id)
            {
                throw new IdDosntExists("the id could not be found", id);
            }
            return station;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations()
        {
            return DataSource.Stations.FindAll(s => !s.IsDeleted);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> expr)
        {
            return DataSource.Stations.FindAll(s => !s.IsDeleted && expr(s));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CountStations(Func<Station, bool> expr)
        {
            return DataSource.Stations.Count(s => !s.IsDeleted && expr(s));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStations(Station station)
        {
            /// if the Station wasnt found throw error
            if (Update(DataSource.Stations, station) == -1)
            {
                throw new IdDosntExists("the id could not be found", station.Id);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int id)
        {
            /// if the Station wasnt found throw error
            if (Delete(DataSource.Stations, id) == -1)
            {
                throw new IdDosntExists("the id could not be found", id);
            }
        }
    }
}

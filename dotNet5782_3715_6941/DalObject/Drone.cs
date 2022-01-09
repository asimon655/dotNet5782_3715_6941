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
        public void AddDrone(Drone drone)
        {
            drone.IsDeleted = false;

            // if we find that the id is already taken by another drone 
            if (DataSource.Drones.Any(s => s.Id == drone.Id))
            {
                throw new IdAlreadyExists("the Id Drone is already taken", drone.Id);
            }

            DataSource.Drones.Add(drone);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            Drone drone = DataSource.Drones.Find(s => !s.IsDeleted && s.Id == id);
            /// if the Drone wasnt found throw error
            if (drone.Id != id)
            {
                throw new IdDosntExists("the Id could not be found", id);
            }
            return drone;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones()
        {
            return DataSource.Drones.FindAll(s => !s.IsDeleted);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> expr)
        {
            return DataSource.Drones.FindAll(s => !s.IsDeleted && expr(s));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int CountDrones(Func<Drone, bool> expr)
        {
            return DataSource.Drones.Count(s => !s.IsDeleted && expr(s));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrones(Drone drone)
        {
            // if we cant find that the id we throw error
            if (Update(DataSource.Drones, drone) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", drone.Id);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            // if we cant find that the id we throw error
            if (Delete(DataSource.Drones, id) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", id);
            }
        }
    }
}
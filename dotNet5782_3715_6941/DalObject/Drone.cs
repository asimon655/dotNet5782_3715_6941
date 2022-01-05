using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
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
            
        public IEnumerable<Drone> GetDrones()
        {
            return DataSource.Drones.FindAll(s => !s.IsDeleted);
        }
        public IEnumerable<Drone> GetDrones(Predicate<Drone> expr)
        {
            return DataSource.Drones.FindAll(s => !s.IsDeleted && expr(s));
        }
        public void UpdateDrones(Drone drone)
        {
            // if we cant find that the id we throw error
            if (Update(DataSource.Drones, drone) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", drone.Id);
            }
        }
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
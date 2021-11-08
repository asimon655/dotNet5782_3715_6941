using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
/*
_________ ______            _______
\__   __/(  __  \ |\     /|(       )
   ) (   | (  \  )| )   ( || () () |
   | |   | |   ) || (___) || || || |
   | |   | |   | ||  ___  || |(_)| |
   | |   | |   ) || (   ) || |   | |
___) (___| (__/  )| )   ( || )   ( |
\_______/(______/ |/     \||/     \|



 
 _______  _______ _________ _______  _______  _
(  ___  )(  ____ \\__   __/(       )(  ___  )( (    /|
| (   ) || (    \/   ) (   | () () || (   ) ||  \  ( |
| (___) || (_____    | |   | || || || |   | ||   \ | |
|  ___  |(_____  )   | |   | |(_)| || |   | || (\ \) |
| (   ) |      ) |   | |   | |   | || |   | || | \   |
| )   ( |/\____) |___) (___| )   ( || (___) || )  \  |
|/     \|\_______)\_______/|/     \|(_______)|/    )_)


 
 */
namespace IDAL
{

    namespace DO
    {

        public struct Drone
        {
            public int Id { set; get; }
            public String Modle { set; get; }
            public WeightCategories MaxWeigth { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Model: " + this.Modle.ToString() + " MaxWeight: " + this.MaxWeigth.ToString(); /// returns strings with all the args of the struct in string  
            }
        }

    }
}
namespace DAL
{
    namespace DalObject
    {
        public partial class DalObject : IDAL.Idal
        {
            public void AddDrone(Drone drone)
            {
                // if we find that the id is already taken by another drone 
                if (DataSource.Drones.Any(s => s.Id == drone.Id))
                {
                    throw new IdAlreadyExists("the Id Drone is already taken", drone.Id);
                }

                DataSource.Drones.Add(drone);
            }
            
            public Drone? PullDataDrone(int id)
            {
                Drone drone = DataSource.Drones.Find(s => s.Id == id);
                /// if the Drone wasnt found throw error
                if (drone.Id != id)
                {
                    throw new IdDosntExists("the Id could not be found", id);
                }
                return drone;
            }
            
            public IEnumerable<Drone> DronesPrint()
            {
                return DAL.DalObject.DataSource.Drones;
            }
            public void UpdateDrones(Drone drone)
            {
                // if we cant find that the id we throw error
                if (!DataSource.Drones.Any(s => s.Id == id))
                {
                    throw new IdDosntExists("the Id Drone is dosnt exists", drone.Id);
                }
                Update<Drone>(DataSource.Drones, drone);
            }
        }
    }
}
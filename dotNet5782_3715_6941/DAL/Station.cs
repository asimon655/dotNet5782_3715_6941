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
        public struct Station
        {
            public int Id { set; get; }
            public int Name { set; get; }
            public double Lattitude { set; get; }
            public double Longitude { set; get; }
            public int ChargeSlots { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Name: " + this.Name.ToString() + " Latitude: " + this.Lattitude.ToString() + " Longitude:" + this.Longitude.ToString()+ "Sexagesimal show of longitude  and lattitude :  " + DAL.DalObject.DalObject.DecimalToSexagesimal(Longitude,Lattitude)+ " ChargeSlots: " + this.ChargeSlots.ToString(); /// returns strings with all the args of the struct in string and longitude and lattiude in Sexagesimal show 
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
            public void AddStation(Station station)
            {
                if (DataSource.Stations.Any(s => s.Id == station.Id))
                {
                    throw new IdAlreadyExists("the station Id is already taken", station.Id);
                }

                DataSource.Stations.Add(station);
            }
            public Station PullDataStation(int id)
            {
                Station station = DataSource.Stations.Find(s => s.Id == id);
                /// if the Station wasnt found throw error
                if (station.Id != id)
                {
                    throw new IdDosntExists("the id could not be found", id);
                }
                return station;
            }
            public IEnumerable<Station> StationsPrint()
            {
                return DAL.DalObject.DataSource.Stations;
            }
            public void UpdateStations(Station station)
            {
                /// if the Station wasnt found throw error
                if (!DataSource.Stations.Any(s => s.Id == station.Id))
                {
                    throw new IdDosntExists("the id could not be found", station.Id);
                }

                Update<Station>(DataSource.Stations, station);
            }
        }
    }
}
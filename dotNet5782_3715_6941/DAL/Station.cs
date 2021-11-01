using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            public double Latitude { set; get; }
            public double Longitude { set; get; }
            public int ChargeSlots { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Name: " + this.Name.ToString() + " Latitude: " + this.Latitude.ToString() + " Longitude:" + this.Longitude.ToString()+ "Sexagesimal show of longitude  and lattitude :  " + DAL.DalObject.DalObject.DecimalToSexagesimal(Longitude,Latitude)+ " ChargeSlots: " + this.ChargeSlots.ToString(); /// returns strings with all the args of the struct in string and longitude and lattiude in Sexagesimal show 
            }
        }

    }
}
namespace DAL
{
    namespace DalObject
    {
        partial class DalObject
        {
            static public void AddStaion(Station cloned)
            {
                Station? exists = PullDataStation(cloned.Id);

                if (exists is null)
                {
                    throw new Exception("the Id Costumer is already taken");
                }

                DataSource.Stations.Add(cloned);
            }
            static public Station? PullDataStation(int _id)
            {
                Station station = DataSource.Stations.Find(s => s.Id == _id);
                /// if the Station wasnt found return null
                if (station.Id != _id)
                    return null;
                return station;
            }
            static public IEnumrable<Station> StaionsPrint()
            {
                return DAL.DalObject.DataSource.Stations;
            }
        }
    }
}
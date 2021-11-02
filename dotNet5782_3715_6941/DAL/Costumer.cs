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
        public struct Costumer
        {
            public int Id { set; get; }
            public string Name { set; get; }
            public string Phone { set; get; }
            public double Longitude { set; get; }
            public double Lattitude { set; get; }

            public override string ToString()
            {
                return "ID: " + this.Id.ToString() + " Name: " + this.Name + " Phone: " + Phone + " Latitude: " + this.Lattitude.ToString() + " Longitude:" + this.Longitude.ToString()+"Sexagesimal show of longitude  and lattitude :  " + DAL.DalObject.DalObject.DecimalToSexagesimal(Longitude,Lattitude); /// returns strings with all the args of the struct in string and longitude and lattiude in Sexagesimal show 
            }
        }
        
    }
}
namespace DAL
{
    namespace DalObject
    {
        partial class DalObject : IDAL.Idal
        {
            public void AddCostumer(Costumer costumer)
            {
                Costumer? exists = PullDataCostumer(costumer.Id);

                if (exists is null)
                {
                    throw new Exception("the Id Costumer is already taken");
                }

                DataSource.Costumers.Add(costumer);
            }
            public Costumer? PullDataCostumer(int id)
            {
                Costumer costumer = DataSource.Costumers.Find(s => s.Id == id);
                /// if the Costumer wasnt found return null
                if (costumer.Id != id)
                    return null;
                return costumer;
            }
            public IEnumerable<Costumer> CostumersPrint()
            {
                return DAL.DalObject.DataSource.Costumers;
            }
        }
    }
}
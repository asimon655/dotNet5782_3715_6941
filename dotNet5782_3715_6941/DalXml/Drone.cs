using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        Drone XmlToDrone(XElement elem)
        {
            return new Drone() {
                Id= int.Parse(elem.Element("Id").Value),
                Modle= elem.Element("Modle").Value,
                MaxWeigth= (WeightCategories)Enum.Parse(typeof(WeightCategories), elem.Element("MaxWeigth").Value),
            };
        }
        XElement DroneToXml(Drone drone)
        {
            return new XElement("Drone", new XElement[] {
                new XElement("Id", drone.Id),
                new XElement("Modle", drone.Modle),
                new XElement("MaxWeigth", drone.MaxWeigth),
            });
        }
        XElement ReadDroneXml()
        {
            try
            {
                return XElement.Load(Path.Combine("Data", fileNames[typeof(Drone)]));
            }
            catch
            {
                return new XElement("ArrayOfDrone");
            }
        }

        void WriteDroneXml(XElement data)
        {
            try
            {
                data.Save(Path.Combine("Data", fileNames[typeof(Drone)]));
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Data");
                data.Save(Path.Combine("Data", fileNames[typeof(Drone)]));
            }
        }


        
        public void AddDrone(Drone drone)
        {
            XElement data = ReadDroneXml();

            if ((from x in data.Elements()
                 where int.Parse(x.Element("Id").Value) == drone.Id
                 select true)
                 .Any())
                throw new IdAlreadyExists("the Id Drone is already taken", drone.Id);

            data.Add(DroneToXml(drone));

            WriteDroneXml(data);
        }

        public Drone GetDrone(int id)
        {
            XElement data = XElement.Load(Path.Combine("Data", fileNames[typeof(Drone)]));
            Drone drone = (from x in data.Elements()
                           where int.Parse(x.Element("Id").Value) == id
                           select XmlToDrone(x))
                           .FirstOrDefault();

            if (drone.Id != id)
                throw new IdDosntExists("the Id could not be found", id);
            
            return drone;
        }

        public IEnumerable<Drone> GetDrones()
        {
            XElement data = XElement.Load(Path.Combine("Data", fileNames[typeof(Drone)]));

            IEnumerable<Drone> drones = from drone in data.Elements()
                                        select XmlToDrone(drone);
            
            return drones;
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> expr)
        {
            XElement data = XElement.Load(Path.Combine("Data", fileNames[typeof(Drone)]));

            IEnumerable<Drone> dronys = from drone in data.Elements()
                                        where expr(XmlToDrone(drone))
                                        select XmlToDrone(drone);
            
            return dronys;
        }

        public void UpdateDrones(Drone drone)
        {
            XElement data = XElement.Load(Path.Combine("Data", fileNames[typeof(Drone)]));

            XElement _drone = (from x in data.Elements()
                               where int.Parse(x.Element("Id").Value) == drone.Id
                               select x).FirstOrDefault();

            if (_drone == default(XElement))
                throw new IdDosntExists("the Id could not be found", drone.Id);

            _drone.ReplaceWith(DroneToXml(drone));

            data.Save(Path.Combine("Data", fileNames[typeof(Drone)]));
        }
    }
}
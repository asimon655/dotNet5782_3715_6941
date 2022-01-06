using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
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
        Drone XmlToDrone(XElement elem)
        {
            return new Drone() {
                Id= int.Parse(elem.Element("Id").Value),
                Modle= elem.Element("Modle").Value,
                MaxWeigth = (WeightCategories)Enum.Parse(typeof(WeightCategories), elem.Element("MaxWeigth").Value),
                IsDeleted = bool.Parse(elem.Element("IsDeleted").Value),
            };
        }
        XElement DroneToXml(Drone drone)
        {
            return new XElement("Drone", new XElement[] {
                new XElement("Id", drone.Id),
                new XElement("Modle", drone.Modle),
                new XElement("MaxWeigth", drone.MaxWeigth),
                new XElement("IsDeleted", drone.IsDeleted),
            });
        }

        private static int IdOf(XElement x)
        {
            return int.Parse(x.Element("Id").Value);
        }

        private static bool IsDeletedOf(XElement x)
        {
            return bool.Parse(x.Element("IsDeleted").Value);
        }

        public void AddDrone(Drone drone)
        {
            drone.IsDeleted = false;

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
            XElement data = ReadDroneXml();
            Drone drone = (from x in data.Elements()
                           where !IsDeletedOf(x) && IdOf(x) == id
                           select XmlToDrone(x))
                           .FirstOrDefault();

            if (drone.Id != id)
                throw new IdDosntExists("the Id could not be found", id);

            return drone;
        }

        public IEnumerable<Drone> GetDrones()
        {
            XElement data = ReadDroneXml();

            IEnumerable<Drone> drones = from drone in data.Elements()
                                        where !IsDeletedOf(drone)
                                        select XmlToDrone(drone);
            
            return drones;
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> expr)
        {
            XElement data = ReadDroneXml();

            IEnumerable<Drone> dronys = from drone in data.Elements()
                                        where !IsDeletedOf(drone) && expr(XmlToDrone(drone))
                                        select XmlToDrone(drone);
            
            return dronys;
        }

        public int CountDrones(Func<Drone, bool> expr)
        {
            XElement data = ReadDroneXml();
            return (from drone in data.Elements()
                    where !IsDeletedOf(drone) && expr(XmlToDrone(drone))
                    select true).Count();
        }

        public void UpdateDrones(Drone drone)
        {
            XElement data = ReadDroneXml();

            XElement _drone = (from x in data.Elements()
                               where !IsDeletedOf(x) && IdOf(x) == drone.Id
                               select x).FirstOrDefault();

            if (_drone == default(XElement))
                throw new IdDosntExists("the Id could not be found", drone.Id);

            _drone.ReplaceWith(DroneToXml(drone));

            WriteDroneXml(data);
        }
        public void DeleteDrone(int id)
        {
            XElement data = ReadDroneXml();

            XElement _drone = (from x in data.Elements()
                               where !IsDeletedOf(x) && IdOf(x) == id
                               select x).FirstOrDefault();

            if (_drone == default(XElement))
                throw new IdDosntExists("the Id could not be found", id);

            _drone.Element("IsDeleted").SetValue(true);

            WriteDroneXml(data);
        }
    }
}
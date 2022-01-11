using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace InitalizeXml
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSource = Type.GetType($"Dal.DataSource, DalObject");

            var DataSourceConfig = dataSource.GetNestedType("Config", BindingFlags.NonPublic | BindingFlags.Static);

            var initalize = DataSourceConfig.GetMethod("Initalize", BindingFlags.NonPublic | BindingFlags.Static);

            initalize.Invoke(null, null);

            var dronesField = dataSource.GetField("Drones", BindingFlags.NonPublic | BindingFlags.Static);
            var customersField = dataSource.GetField("Costumers", BindingFlags.NonPublic | BindingFlags.Static);
            var stationsField = dataSource.GetField("Stations", BindingFlags.NonPublic | BindingFlags.Static);
            var parcelsField = dataSource.GetField("Parcels", BindingFlags.NonPublic | BindingFlags.Static);
            var dronesChargesField = dataSource.GetField("DronesCharges", BindingFlags.NonPublic | BindingFlags.Static);

            List<Drone> Drones = (List<Drone>)dronesField.GetValue(null);
            List<Customer> Costumers = (List<Customer>)customersField.GetValue(null);
            List<Station> Stations = (List<Station>)stationsField.GetValue(null);
            List<Parcel> Parcels = (List<Parcel>)parcelsField.GetValue(null);
            List<DroneCharge> DronesCharges = (List<DroneCharge>)dronesChargesField.GetValue(null);

            XmlSerializer dronesSer = new XmlSerializer(typeof(List<Drone>));
            XmlSerializer customersSer = new XmlSerializer(typeof(List<Customer>));
            XmlSerializer stationsSer = new XmlSerializer(typeof(List<Station>));
            XmlSerializer parcelsSer = new XmlSerializer(typeof(List<Parcel>));
            XmlSerializer dronesChargesSer = new XmlSerializer(typeof(List<DroneCharge>));

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            TextWriter dronesWri = new StreamWriter(Path.Combine("Data", "Drones.xml"));
            TextWriter customersWri = new StreamWriter(Path.Combine("Data", "Customers.xml"));
            TextWriter stationsWri = new StreamWriter(Path.Combine("Data", "Stations.xml"));
            TextWriter parcelWri = new StreamWriter(Path.Combine("Data", "Parcels.xml"));
            TextWriter dronesChargesWri = new StreamWriter(Path.Combine("Data", "DronesCharges.xml"));

            try
            {
                dronesSer.Serialize(dronesWri, Drones);
                customersSer.Serialize(customersWri, Costumers);
                stationsSer.Serialize(stationsWri, Stations);
                parcelsSer.Serialize(parcelWri, Parcels);
                dronesChargesSer.Serialize(dronesChargesWri, DronesCharges);
            }
            catch (Exception) { throw; }
            finally
            {
                dronesWri.Close();
                customersWri.Close();
                stationsWri.Close();
                parcelWri.Close();
                dronesChargesWri.Close();
            }

            var PowerConsumptionFreeField = DataSourceConfig.GetField("PowerConsumptionFree", BindingFlags.NonPublic | BindingFlags.Static);
            var PowerConsumptionLightField = DataSourceConfig.GetField("PowerConsumptionLight", BindingFlags.NonPublic | BindingFlags.Static);
            var PowerConsumptionMediumField = DataSourceConfig.GetField("PowerConsumptionMedium", BindingFlags.NonPublic | BindingFlags.Static);
            var PowerConsumptionHeavyField = DataSourceConfig.GetField("PowerConsumptionHeavy", BindingFlags.NonPublic | BindingFlags.Static);
            var ChargingSpeedField = DataSourceConfig.GetField("ChargingSpeed", BindingFlags.NonPublic | BindingFlags.Static);
            var IdCreationField = DataSourceConfig.GetField("IdCreation", BindingFlags.NonPublic | BindingFlags.Static);

            double PowerConsumptionFree = (double)PowerConsumptionFreeField.GetValue(null);
            double PowerConsumptionLight = (double)PowerConsumptionLightField.GetValue(null);
            double PowerConsumptionMedium = (double)PowerConsumptionMediumField.GetValue(null);
            double PowerConsumptionHeavy = (double)PowerConsumptionHeavyField.GetValue(null);
            double ChargingSpeed = (double)ChargingSpeedField.GetValue(null);
            int IdCreation = (int)IdCreationField.GetValue(null);

            XElement XmlConfig = new XElement("config", new XElement[] {
                new XElement("power-usage", new XElement[] { 
                    new XElement("PowerConsumptionFree", PowerConsumptionFree),
                    new XElement("PowerConsumptionLight", PowerConsumptionLight),
                    new XElement("PowerConsumptionMedium", PowerConsumptionMedium),
                    new XElement("PowerConsumptionHeavy", PowerConsumptionHeavy),
                    new XElement("ChargingSpeed", ChargingSpeed)
                }),
                new XElement("parcel-index", IdCreation)
            });

            XmlConfig.Save(Path.Combine("Data", "config.xml"));

            Console.WriteLine("succusfully initlize the xml files from DataSource");
        }
    }
}

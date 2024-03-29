﻿using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        public DalXml() { }

        static DalXml() { }
        internal static readonly object padlock = new object();
        internal static DalXml instance;

        private static DalXml Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DalXml();
                        }
                    }
                }
                return instance;
            }
        }

        public double[] GetPowerConsumption()
        {
            return XmlConfig.GetPowerConsts();
        }

        private readonly Dictionary<Type, string> fileNames = new Dictionary<Type, string>() {
                {typeof(Customer) , "Customers.xml"},
                {typeof(Drone) , "Drones.xml"},
                {typeof(DroneCharge) , "DronesCharges.xml"},
                {typeof(Parcel) , "Parcels.xml"},
                {typeof(Station) , "Stations.xml"},
                {typeof(DronePic) , "DronePics.xml"},
                {typeof(CustomerPic) , "CustomerPics.xml"},
            };

        private List<T> Read<T>()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<T>));
            XmlReader reader;
            List<T> data;

            try
            {
                reader = new XmlTextReader(Path.Combine("Data", fileNames[typeof(T)]));

                if (ser.CanDeserialize(reader))
                {
                    data = (List<T>)ser.Deserialize(reader);
                }
                else
                {
                    data = new List<T>();
                }
                reader.Close();

                return data;
            }
            catch
            {
                return new List<T>();
            }
        }

        private void Write<T>(List<T> data)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<T>));

            TextWriter writer;
            try
            {
                writer = new StreamWriter(Path.Combine("Data", fileNames[typeof(T)]));
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Data");
                writer = new StreamWriter(Path.Combine("Data", fileNames[typeof(T)]));
            }
            try
            {
                ser.Serialize(writer, data);
            }
            catch (Exception) { throw; }
            finally
            {
                writer.Close();
            }
        }

        private static int Update<T>(List<T> listy, T updater)
        {
            var id = typeof(T).GetProperty("Id");
            var isDeleted = typeof(T).GetProperty("IsDeleted");

            int updaterId = (int)id.GetValue(updater, null);

            int index = listy.FindIndex(x => !(bool)isDeleted.GetValue(x, null) && (int)id.GetValue(x, null) == updaterId);

            if (index != -1)
            {
                listy[index] = updater;
            }

            return index;
        }

        private static int Delete<T>(List<T> listy, int deleteId)
        {
            var id = typeof(T).GetProperty("Id");
            var isDeleted = typeof(T).GetProperty("IsDeleted");

            int index = listy.FindIndex(x => !(bool)isDeleted.GetValue(x, null) && (int)id.GetValue(x, null) == deleteId);

            if (index != -1)
            {
                object updater = listy[index];
                isDeleted.SetValue(updater, true);
                listy[index] = (T)updater;
            }

            return index;
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dal
{
    class XmlConfig
    {
        private static XElement ReadConfigXml()
        {
            try
            {
                return XElement.Load(Path.Combine("Data", "config.xml"));
            }
            catch
            {
                throw new Exception("i need the config file to run");
            }
        }

        private static void WriteConfigXml(XElement dalConfig)
        {
            try
            {
                dalConfig.Save(Path.Combine("Data", "config.xml"));
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Data");
                dalConfig.Save(Path.Combine("Data", "config.xml"));
            }
        }


        public static double[] GetPowerConsts()
        {
            XElement dalConfig = ReadConfigXml();
            return (from num in dalConfig.Element("power-usage").Elements()
                    select Convert.ToDouble(num.Value)).ToArray();
        }

        public static int GetPromoteParcelIndex()
        {
            XElement dalConfig = ReadConfigXml();
            int index = Convert.ToInt32(dalConfig.Element("parcel-index").Value);
            dalConfig.Element("parcel-index").SetValue(index + 1);
            WriteConfigXml(dalConfig);
            return index;
        }
    }
}

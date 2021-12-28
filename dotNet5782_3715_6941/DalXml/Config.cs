using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    class XmlConfig
    {
        public static double[] GetPowerConsts()
        {
            XElement dalConfig = XElement.Load(Path.Combine("Data", "config.xml"));
            return (from num in dalConfig.Element("power-usage").Elements()
                           select Convert.ToDouble(num.Value)).ToArray();
        }
        public static int GetPromoteParcelIndex()
        {
            XElement dalConfig = XElement.Load(Path.Combine("Data", "config.xml"));
            int index = Convert.ToInt32(dalConfig.Element("parcel-index").Value);
            dalConfig.Element("parcel-index").SetValue(index+1);
            dalConfig.Save(Path.Combine("Data", "config.xml"));
            return index;
        }
    }
}

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
    }
}

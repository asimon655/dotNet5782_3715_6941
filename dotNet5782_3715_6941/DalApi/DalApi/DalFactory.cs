using System.Reflection;
using System;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal()
        {
            string dalType = DalConfig.DalName;
            string dalPkg = DalConfig.DalPackages[dalType];

            if (dalPkg == null) throw new DalConfigException($"the package {dalType} is not found in package list in dal-config.xml");

            try
            {
                Assembly.Load(dalPkg);
            }
            catch (Exception err)
            {
                throw new DalConfigException("failed to load the dal-config.xml file", err);
            }

            Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");

            if (type is null) throw new DalConfigException($"the type {dalPkg} could not be found in {dalPkg}.dll");

            IDal dal = (IDal)type.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);

            if (dal is null) throw new DalConfigException($"class {dalType} is not singleton or wrong property name for Instance");

            return dal;
        }
    }
}
using DalApi;
using System.Reflection;
using System;

namespace DalFactory
{
    public static class DalFactory
    {
        public static IDal GetDal(string typename)
        {
            string pathPrefix = AppDomain.CurrentDomain.BaseDirectory;
            Assembly assembly = Assembly.LoadFrom(pathPrefix + typename + ".dll");
            if (assembly is null)
            {
                throw new Exception("couldnt find the dll file");
            }
            Type type = assembly.GetType("Dal." + typename);
            if (type is null)
            {
                throw new Exception("couldnt find the type");
            }
            // IDal dal = (IDal)Activator.CreateInstance(type);
            IDal dal = (IDal)type.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            if (dal is null)
            {
                throw new Exception("couldnt convert the type to IDal");
            }
            return dal;
        }
    }
}
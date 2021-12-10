using DalApi;
using System.Reflection;
using System;

namespace DalFactory
{
    public static class DalFactory
    {
        public static IDal GetDal(string filename, string typename)
        {
            string pathPrefix = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(pathPrefix);
            Assembly assembly = Assembly.LoadFrom(pathPrefix + filename);
            if (assembly is null)
            {
                throw new Exception("couldnt find the dll file");
            }
            Type type = assembly.GetType(typename);
            if (type is null)
            {
                throw new Exception("couldnt find the type");
            }
            IDal dal = (IDal)Activator.CreateInstance(type);
            if (dal is null)
            {
                throw new Exception("couldnt convert the type to IDal");
            }
            return dal;
        }
    }
}
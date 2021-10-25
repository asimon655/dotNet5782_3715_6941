using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    class SysFunc
    {
        public static void printArray<T>(T[] list)
        {

            foreach (T item in list)
                if (!(item.Equals(default(T))))
                    Console.WriteLine(item.ToString());

        }
        public static void printList<T>(List<T> list)
        {

            foreach (T item in list)
                if (!(item.Equals(default(T))))
                    Console.WriteLine(item.ToString());

        }
        public static void printEnum<T>() where T : struct, IConvertible
        {
            foreach (T i in Enum.GetValues(typeof(T)))
                Console.WriteLine(Convert.ToUInt32(i).ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<T>((T)i));
            

        }

    }
}



﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 
 
    // | |
   //__| |     ___     ( )  _   __      ___       __
  / ___  |   ((   ) ) / / // ) )  ) ) //   ) ) //   ) )
 //    | |    \ \    / / // / /  / / //   / / //   / /
//     | | //   ) ) / / // / /  / / ((___/ / //   / /
 
 
 
 
 
    ___   ___
      / /    //    ) ) //    / / /|    //| |
     / /    //    / / //___ / / //|   // | |
    / /    //    / / / ___   / // |  //  | |
   / /    //    / / //    / / //  | //   | |
__/ /___ //____/ / //    / / //   |//    | |

 */
namespace ConsoleUI
{
    class SysFunc 
    {

        public static  int SafeEnterUInt(String prefix = "")///enters int ( it reccimends to uint but the func do not insist with the client) and if it cants parse it , it shows error massage and thre is a option to print inputing  message like py input  
        {
            Console.Write(prefix);
            int num;
            return (int.TryParse(Console.ReadLine(), out num) ? num : SafeEnterUInt("you need to enter only numbers from N (Z will accept but will be not realistic)! Try again to enter the field "));

        }

        public static double SafeEnterDouble(String prefix = "")///enters double and if it cants parse it , it shows error massage and thre is a option to print inputing  message like py input  
        {
            Console.Write(prefix);
            double num;
            return (double.TryParse(Console.ReadLine(), out num) ? num : SafeEnterDouble("you need to enter only numbers from Q! Try again to enter the field "));

        }
        /// from here all is generic functions (i am hoping  that you are not getting mad ;) ) 
        /// <summary>
        /// Console inp / out funcs - i used it because yair landed on me and simon that command 
        /// </summary>
        public static void printList<T>(List<T> list)//prints listof something and kicks out all the deafult things 
        {

            foreach (T item in list)
                if (!(item.Equals(default(T))))
                    Console.WriteLine(item.ToString());

        }
        public static void printEnum<T>() where T : struct, IConvertible //prints all the enum options  of any enum (where T: struct Icon.... promiss that this is doing that )  and kicks out all the deafult things 
        {
            foreach (T i in Enum.GetValues(typeof(T)))
                Console.WriteLine(Convert.ToUInt32(i).ToString() + ".) " + i.ToString());
            ///foreach (T i in Enum.GetValues(typeof(T))) return alll the enums that exsist in that enum type 
            ///Convert.ToUInt32(i).ToString() - force convert to int because otherwise it do not work  - that is the most powerull and basic convert 
            ///DAL.DalObject.EnumHelper.GetDescription<T>((T)i) => my functuion that returns the descrption 

        }

    }
}



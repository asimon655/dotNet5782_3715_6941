using System;
using System.ComponentModel;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {

            int enter = 1;

            while ((enter != -1))
            {
                Console.WriteLine("please enter a number from the menue ");
                for (int i = -1; i < 4; i++)
                    Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Menu>((IDAL.DO.Menu)i));
                Console.WriteLine((int.TryParse(Console.ReadLine(), out enter ) ? "" : "please enter only numbers"));
                Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Menu>((IDAL.DO.Menu)enter));
                switch (enter)
                {



                    case (int)IDAL.DO.Menu.Add:
                        break;
                    case (int)IDAL.DO.Menu.Details:
                        break;
                    case (int)IDAL.DO.Menu.Update:
                        break;
                    case (int)IDAL.DO.Menu.ListShow:
                        break;
                    case (int)IDAL.DO.Menu.exit:
                        break;

                }
            }

        }
    }
}

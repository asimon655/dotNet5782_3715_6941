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
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i =0 ; i < 4; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Add.Costumer:
                                break;
                            case (int)IDAL.DO.Add.Drone:
                                break;
                            case (int)IDAL.DO.Add.Package:
                                break;
                            case (int)IDAL.DO.Add.Staion:
                                break;

                           }
                        break;
                    case (int)IDAL.DO.Menu.Details:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < 4; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Add.Costumer:
                                break;
                            case (int)IDAL.DO.Add.Drone:
                                break;
                            case (int)IDAL.DO.Add.Package:
                                break;
                            case (int)IDAL.DO.Add.Staion:
                                break;

                        }
                        break;
                    case (int)IDAL.DO.Menu.Update:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < 4; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Add.Costumer:
                                break;
                            case (int)IDAL.DO.Add.Drone:
                                break;
                            case (int)IDAL.DO.Add.Package:
                                break;
                            case (int)IDAL.DO.Add.Staion:
                                break;

                        }
                        break;
                    case (int)IDAL.DO.Menu.ListShow:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < 4; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Add.Costumer:
                                break;
                            case (int)IDAL.DO.Add.Drone:
                                break;
                            case (int)IDAL.DO.Add.Package:
                                break;
                            case (int)IDAL.DO.Add.Staion:
                                break;

                        }
                        break;
                    case (int)IDAL.DO.Menu.exit:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < 4; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Add.Costumer:
                                break;
                            case (int)IDAL.DO.Add.Drone:
                                break;
                            case (int)IDAL.DO.Add.Package:
                                break;
                            case (int)IDAL.DO.Add.Staion:
                                break;

                        }
                        break;

                }
            }

        }
    }
}

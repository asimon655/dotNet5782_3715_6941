using System;
using System.ComponentModel;
/*  
 __o__   o__ __o        o         o    o          o
   |    <|     v\      <|>       <|>  <|\        /|>
  / \   / \     <\     < >       < >  / \\o    o// \
  \o/   \o/       \o    |         |   \o/ v\  /v \o/
   |     |         |>   o__/_ _\__o    |   <\/>   |
  < >   / \       //    |         |   / \        / \
   |    \o/      /     <o>       <o>  \o/        \o/
   o     |      o       |         |    |          |
 __|>_  / \  __/>      / \       / \  / \        / \

    _        _
   / \   ___(_)_ __ ___   ___  _ __
  / _ \ / __| | '_ ` _ \ / _ \| '_ \
 / ___ \\__ \ | | | | | | (_) | | | |
/_/   \_\___/_|_| |_| |_|\___/|_| |_|



 */
//Note: if you want to add DroneId to Parcel you can do that only in bind (update) and not in Add (when you create the Parcel )
namespace ConsoleUI
{
    class Program
    {
        static int SafeEnterUInt(bool print=false)
        {
            if (print)
                Console.WriteLine("you need to enter only numbers from N (Z will accept but will be not realistic)! Try again to enter the field ");
            int num;
            return (int.TryParse(Console.ReadLine(), out num) ? num : SafeEnterUInt(true)); 

        }

        static double  SafeEnterDouble(bool print=false )
        {
            if (print)
                Console.WriteLine("you need to enter only numbers from Q! Try again to enter the field ");
            double num;
            return (double.TryParse(Console.ReadLine(), out num) ? num : SafeEnterDouble(true)); 

        }
        static void Main(string[] args)
        {
          

            int enter = 1;
  
            while ((enter != 4))
            {
                Console.WriteLine("please enter a number from the menue ");
                for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.Menu)).Length; i++)
                    Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Menu>((IDAL.DO.Menu)i));
                Console.WriteLine((int.TryParse(Console.ReadLine(), out enter ) ? "" : "please enter only numbers"));
                Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Menu>((IDAL.DO.Menu)enter));
                switch (enter)
                {



                    case (int)IDAL.DO.Menu.Add:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i =0 ; i < Enum.GetNames(typeof(IDAL.DO.Add)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Add>((IDAL.DO.Add)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Add.Costumer:
                                IDAL.DO.Costumer tmp = new IDAL.DO.Costumer();

                                Console.WriteLine("enter detials of choosen object   \nLattitude \nLongitude \nName\nPhone ");
                                
                                tmp.Lattitude = SafeEnterDouble();
                                tmp.Longitude = SafeEnterDouble();
                                tmp.Name = Console.ReadLine();
                                tmp.Phone = Console.ReadLine();
                                DAL.DalObject.DalObject.AddCostumer(tmp);
                                break;
                            case (int)IDAL.DO.Add.Drone:
                                Console.WriteLine("enter detials of choosen object   \nBattery  \nMaxweight\nModle \nStatus  ");
                                IDAL.DO.Drone tmp1 = new IDAL.DO.Drone();
                                tmp1.Battery = SafeEnterDouble();
                    
                                Console.WriteLine("please enter a number from the menue ");
                                for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.WeightCategories)).Length; i++)
                                    Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>((IDAL.DO.WeightCategories)i));
                                tmp1.MaxWeigth = (IDAL.DO.WeightCategories)SafeEnterUInt(); ;
                                tmp1.Modle = Console.ReadLine();
                                Console.WriteLine("please enter a number from the menue ");
                                tmp1.Status = (IDAL.DO.DroneStatuses)IDAL.DO.DroneStatuses.Free ;
                                break;
                            case (int)IDAL.DO.Add.Package:
                                Console.WriteLine("enter detials of choosen object   weight and priorty (weight will show up first)"  );
                                IDAL.DO.Parcel tmp2 = new IDAL.DO.Parcel();
                                for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.Priorities)).Length; i++)
                                    Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Priorities>((IDAL.DO.Priorities)i));
                               
                                Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Priorities>((IDAL.DO.Priorities)enter));
                                tmp2.Priority = (IDAL.DO.Priorities)SafeEnterUInt(); 
                                Console.WriteLine("please enter a number from the menue ");
                                for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.WeightCategories)).Length; i++)
                                    Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.WeightCategories>((IDAL.DO.WeightCategories)i));
                                tmp2.Weight = (IDAL.DO.WeightCategories)SafeEnterUInt(); ;
                                DAL.DalObject.DalObject.AddParcel(tmp2);
                                break;
                            case (int)IDAL.DO.Add.Staion:
                                Console.WriteLine("enter detials of choosen object  \nId \nLatitude  \nLongitude \nName ");
                                IDAL.DO.Staion tmp3 = new IDAL.DO.Staion();
                                tmp3.Id = SafeEnterUInt(); ;
                                tmp3.Latitude = SafeEnterUInt(); ;
                                tmp3.Longitude = SafeEnterUInt(); ;
                                tmp3.Name = SafeEnterUInt(); ;
                                DAL.DalObject.DalObject.AddStaion(tmp3);
                                break;

                           }
                        break;
                    case (int)IDAL.DO.Menu.Details:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.Details)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Details>((IDAL.DO.Details)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Details>((IDAL.DO.Details)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Details.BaseStaion:
                                Console.WriteLine("enter Staion id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DAL.DalObject.DalObject.PullDataStaion(enter).Equals(null) ? "No Object found with that ID please try again sir": DAL.DalObject.DalObject.PullDataStaion(enter)));
                                break;
                            case (int)IDAL.DO.Details.Costumer:
                                Console.WriteLine("enter Costumer id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DAL.DalObject.DalObject.PullDataCostumer(enter).Equals(null) ? "No Object found with that ID please try again sir": DAL.DalObject.DalObject.PullDataCostumer(enter)));
                                break;
                            case (int)IDAL.DO.Details.Drone:
                                Console.WriteLine("enter Drone id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DAL.DalObject.DalObject.PullDataDrone(enter).Equals(null)? "No Object found with that ID please try again sir":DAL.DalObject.DalObject.PullDataDrone(enter)));
                                break;
                            case (int)IDAL.DO.Details.Package:
                                Console.WriteLine("enter Parcel id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DAL.DalObject.DalObject.PullDataParcel(enter).Equals(null) ? "No Object found with that ID please try again sir": DAL.DalObject.DalObject.PullDataParcel(enter)));
                                break;

                        }
                        break;
                    case (int)IDAL.DO.Menu.Update:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.Update)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Update>((IDAL.DO.Update)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.Update>((IDAL.DO.Update)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.Update.PackgeandDrone:
                                Console.WriteLine("enter Parcel \nParcek id \nTarget id \nSender id ");
                                enter = SafeEnterUInt();
                               
                                DAL.DalObject.DalObject.BindParcelToDrone(enter, SafeEnterUInt(),SafeEnterUInt());
                                Console.WriteLine("First valid drone will serve you sir ");
                                break;

                            case (int)IDAL.DO.Update.PackgeTakeDrone:
                                Console.WriteLine("enter the id of the package you wnat to pickup");
                                DAL.DalObject.DalObject.PickUpByDrone(SafeEnterUInt());
                                
                               

                                
                                break;
                            case (int)IDAL.DO.Update.PackgeTakeCostumer:
                                Console.WriteLine("Enter the package  ID please : ");
                                DAL.DalObject.DalObject.ParcelDeliveredToCostumer(SafeEnterUInt());

                                break;
                            case (int)IDAL.DO.Update.DroneSend:
                                Console.WriteLine("Enter the Drone  ID please : ");
                                DAL.DalObject.DalObject.DroneCharge(SafeEnterUInt());

                                break;
                            case (int)IDAL.DO.Update.DroneRelease:
                                Console.WriteLine("Enter the Drone ID please : ");
                                DAL.DalObject.DalObject.DroneChargeRealse(SafeEnterUInt());
                                break;

                        }
                        break;
                    case (int)IDAL.DO.Menu.ListShow:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < Enum.GetNames(typeof(IDAL.DO.ListShow)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.ListShow>((IDAL.DO.ListShow)i)) ;
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(DAL.DalObject.EnumHelper.GetDescription<IDAL.DO.ListShow>((IDAL.DO.ListShow)enter));
                        switch (enter)
                        {
                            case (int)IDAL.DO.ListShow.BaseStaions:
                                Console.WriteLine("Printing ... ");
                                DAL.DalObject.DalObject.StaionsPrint();
                                break;
                            case (int)IDAL.DO.ListShow.BaseStaionsFreePorts:
                                break;
                            case (int)IDAL.DO.ListShow.Costumers:
                                Console.WriteLine("Printing ... ");
                                DAL.DalObject.DalObject.CostumersPrint();
                                break;
                            case (int)IDAL.DO.ListShow.Drones:
                                Console.WriteLine("Printing ... ");
                                DAL.DalObject.DalObject.DronesPrint();
                                break;
                            case (int)IDAL.DO.ListShow.Packages:
                                Console.WriteLine("Printing ... ");
                                DAL.DalObject.DalObject.ParcelsPrint();
                                break;
                            case (int)IDAL.DO.ListShow.PackagesWithoutDrones:
                                   Console.WriteLine("Printing ... ");
                                DAL.DalObject.DalObject.ParcelsWithotDronesPrint();
                                break;

                        }
                        break;
                    case (int)IDAL.DO.Menu.exit:

                        break;

                }
            }

        }
    }
}

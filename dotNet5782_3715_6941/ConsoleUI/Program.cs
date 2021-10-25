using System;
using System.ComponentModel;
using IDAL.DO;
using DAL.DalObject; 
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
        
        static int SafeEnterUInt(String prefix = "")
        {
            Console.Write(prefix);
            int num;
            return (int.TryParse(Console.ReadLine(), out num) ? num : SafeEnterUInt("you need to enter only numbers from N (Z will accept but will be not realistic)! Try again to enter the field ")); 

        }

        static double  SafeEnterDouble(String prefix="")
        {
            Console.Write(prefix);
            double num;
            return (double.TryParse(Console.ReadLine(), out num) ? num : SafeEnterDouble("you need to enter only numbers from Q! Try again to enter the field ")); 

        }
        static void Main(string[] args)
        {


            int enter;
            int command;
            do
            {
                SysFunc.printEnum<Menu>();
                command = SafeEnterUInt("please enter a number from the menue ");
                Console.WriteLine(EnumHelper.GetDescription<Menu>((Menu)command));
                switch (command)
                {



                    case (int)Menu.Add:
                        SysFunc.printEnum<Add>();
                        enter = SafeEnterUInt("please enter a number from the menue ");
                        Console.WriteLine(EnumHelper.GetDescription<Add>((Add)enter));
                        switch (enter)
                        {
                            case (int)Add.Costumer:
                                Costumer newCostumer = new Costumer();


                                newCostumer.Id = SafeEnterUInt("Id: ");
                                newCostumer.Lattitude = SafeEnterDouble("Lattitude: ");
                                newCostumer.Longitude = SafeEnterDouble("Longitude: ");
                                Console.Write("Name: ");
                                newCostumer.Name = Console.ReadLine();
                                Console.Write("Phone: ");
                                newCostumer.Phone = Console.ReadLine();
                                DalObject.AddCostumer(newCostumer);
                                break;
                            case (int)Add.Drone:
                                
                                Drone DroneItem = new Drone();
                                DroneItem.Id = SafeEnterUInt("Id: ");
                                DroneItem.Battery = SafeEnterDouble("Battery: ");
                                Console.WriteLine("Maxweight=>please enter a number from the menue : ");
                                SysFunc.printEnum<WeightCategories>();
                                DroneItem.MaxWeigth = (WeightCategories)SafeEnterUInt("Maxweight: (Choose from the numbers above:  "); ;
                                Console.Write("Model: ");
                                DroneItem.Modle = Console.ReadLine();
                                DroneItem.Status = (DroneStatuses)DroneStatuses.Free;
                                DalObject.AddDrone(DroneItem);
                                break;
                            case (int)Add.Package:
                          
                                Parcel PackageItem = new Parcel();
                                Console.WriteLine("Priorty=>please enter a number from the menue : ");
                                SysFunc.printEnum<Priorities>();
                                Console.WriteLine(EnumHelper.GetDescription<Priorities>((Priorities)enter));
                                PackageItem.Priority = (Priorities)SafeEnterUInt("enter a number from the numbers above");
                                Console.WriteLine("weight=>please enter a number from the menue : ");
                                SysFunc.printEnum<WeightCategories>();
                                PackageItem.Weight = (WeightCategories)SafeEnterUInt("enter a number from the numbers above : "); ;
                                DalObject.AddParcel(PackageItem);
                                break;
                            case (int)Add.Staion:
                                Station StaionItem = new Station();
                                StaionItem.Id = SafeEnterUInt("Id: ");
                                StaionItem.Name = SafeEnterUInt("Name: ");
                                StaionItem.Latitude = SafeEnterDouble("Latitude: ");
                                StaionItem.Longitude = SafeEnterDouble("Longitude: ");
                                StaionItem.ChargeSlots = SafeEnterUInt("ChargeSlots: ");
                                DalObject.AddStaion(StaionItem);
                                break;

                        }
                        break;
                    case (int)Menu.Details:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < Enum.GetNames(typeof(Details)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + EnumHelper.GetDescription<Details>((Details)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Details>((Details)enter));
                        switch (enter)
                        {
                            case (int)Details.BaseStaion:
                                Console.WriteLine("enter Station Id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataStaion(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataStaion(enter)));
                                break;
                            case (int)Details.Costumer:
                                Console.WriteLine("enter Costumer Id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataCostumer(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataCostumer(enter)));
                                break;
                            case (int)Details.Drone:
                                Console.WriteLine("enter Drone Id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataDrone(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataDrone(enter)));
                                break;
                            case (int)Details.Package:
                                Console.WriteLine("enter Parcel Id please : ");
                                enter = SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataParcel(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataParcel(enter)));
                                break;

                        }
                        break;
                    case (int)Menu.Update:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < Enum.GetNames(typeof(Update)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + EnumHelper.GetDescription<Update>((Update)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Update>((Update)enter));
                        switch (enter)
                        {
                            case (int)Update.PackgeandDrone:
                                DalObject.BindParcelToDrone(SafeEnterUInt("Parcel Id"), SafeEnterUInt("Target Id"), SafeEnterUInt("Sender Id"));
                                Console.WriteLine("First valid drone will serve you sir ");
                                break;

                            case (int)Update.PackgeTakeDrone:
                                DalObject.PickUpByDrone(SafeEnterUInt("enter the Id of the package you wnat to pickup"));




                                break;
                            case (int)Update.PackgeTakeCostumer:
                                Console.WriteLine("Enter the package  ID please : ");
                                DalObject.ParcelDeliveredToCostumer(SafeEnterUInt());

                                break;
                            case (int)Update.DroneSendCharge:
                                Console.WriteLine("Enter the Drone  ID and then Station  ID please : ");
                                DalObject.DroneCharge(SafeEnterUInt(), SafeEnterUInt());

                                break;
                            case (int)Update.DroneRelease:
                                Console.WriteLine("Enter the Drone ID please : ");
                                DalObject.DroneChargeRealse(SafeEnterUInt());
                                break;

                        }
                        break;
                    case (int)Menu.ListShow:
                        Console.WriteLine("please enter a number from the menue ");
                        for (int i = 0; i < Enum.GetNames(typeof(ListShow)).Length; i++)
                            Console.WriteLine(i.ToString() + ".) " + EnumHelper.GetDescription<ListShow>((ListShow)i));
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<ListShow>((ListShow)enter));
                        switch (enter)
                        {
                            case (int)ListShow.BaseStaions:
                                Console.WriteLine("Printing ... ");
                               SysFunc.printArray<Station>( DalObject.StaionsPrint());
                                break;
                            case (int)ListShow.BaseStaionsFreePorts:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<Station>(DalObject.BaseStaionsFreePortsPrint());
                                break;
                            case (int)ListShow.Costumers:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printArray<Costumer>( DalObject.CostumersPrint());
                                break;
                            case (int)ListShow.Drones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printArray<Drone>(DalObject.DronesPrint());
                                break;
                            case (int)ListShow.Packages:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printArray<Parcel>( DalObject.ParcelsPrint());
                                break;
                            case (int)ListShow.PackagesWithoutDrones:
                                Console.WriteLine("Printing ... ");
                               SysFunc.printList<Parcel>( DalObject.ParcelsWithotDronesPrint());
                                break;

                        }
                        break;
                    case (int)Menu.exit:

                        break;

                }
            } while ((command != 4));

        }
    }
}

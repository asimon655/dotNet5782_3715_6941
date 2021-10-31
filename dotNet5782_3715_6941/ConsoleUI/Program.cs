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
     
        static void Main(string[] args)
        {


            int enter;/// for the sub switches 
            int command;
            do
            {
                SysFunc.printEnum<Menu>();
                command = SysFunc.SafeEnterUInt("please enter a number from the menue ");
                Console.WriteLine(EnumHelper.GetDescription<Menu>((Menu)command));
                switch (command)
                {
                    case (int)Menu.Add:
                        /// in all the adds i just create dumy struct T and fill it with vals and call the add method in dal object 
                        SysFunc.printEnum<Add>();
                        enter = SysFunc.SafeEnterUInt("please enter a number from the menue ");
                        Console.WriteLine(EnumHelper.GetDescription<Add>((Add)enter));
                        switch (enter)
                        {
                            case (int)Add.Costumer:
                                Costumer newCostumer = new Costumer();
                                newCostumer.Id = SysFunc.SafeEnterUInt("Id: ");
                                newCostumer.Lattitude = SysFunc.SafeEnterDouble("Lattitude: ");
                                newCostumer.Longitude = SysFunc.SafeEnterDouble("Longitude: ");
                                Console.Write("Name: ");
                                newCostumer.Name = Console.ReadLine();
                                Console.Write("Phone: ");
                                newCostumer.Phone = Console.ReadLine();
                                Console.WriteLine(DalObject.AddCostumer(newCostumer));
                                break;
                            case (int)Add.Drone:
                                
                                Drone DroneItem = new Drone();
                                DroneItem.Id = SysFunc.SafeEnterUInt("Id: ");
                                DroneItem.Battery = SysFunc.SafeEnterDouble("Battery: ");
                                Console.WriteLine("Maxweight=>please enter a number from the menue : ");
                                SysFunc.printEnum<WeightCategories>();
                                DroneItem.MaxWeigth = (WeightCategories)SysFunc.SafeEnterUInt("Maxweight: (Choose from the numbers above:  "); ;
                                Console.Write("Model: ");
                                DroneItem.Modle = Console.ReadLine();
                                DroneItem.Status = (DroneStatuses)DroneStatuses.Free;
                                Console.WriteLine(DalObject.AddDrone(DroneItem));
                                break;
                            case (int)Add.Package:
                          
                                Parcel PackageItem = new Parcel();
                                Console.WriteLine("Priorty=>please enter a number from the menue : ");
                                SysFunc.printEnum<Priorities>();
                                Console.WriteLine(EnumHelper.GetDescription<Priorities>((Priorities)enter));
                                PackageItem.Priority = (Priorities)SysFunc.SafeEnterUInt("enter a number from the numbers above");
                                Console.WriteLine("weight=>please enter a number from the menue : ");
                                SysFunc.printEnum<WeightCategories>();
                                PackageItem.Weight = (WeightCategories)SysFunc.SafeEnterUInt("enter a number from the numbers above : "); ;
                                Console.WriteLine(DalObject.AddParcel(PackageItem));
                                break;
                            case (int)Add.Staion:
                                Station StaionItem = new Station();
                                StaionItem.Id = SysFunc.SafeEnterUInt("Id: ");
                                StaionItem.Name = SysFunc.SafeEnterUInt("Name: ");
                                StaionItem.Latitude = SysFunc.SafeEnterDouble("Latitude: ");
                                StaionItem.Longitude = SysFunc.SafeEnterDouble("Longitude: ");
                                StaionItem.ChargeSlots = SysFunc.SafeEnterUInt("ChargeSlots: ");
                                Console.WriteLine(DalObject.AddStaion(StaionItem));
                                break;

                        }
                        break;
                    case (int)Menu.Details:
                        /// in all the details i just get a drone id and call the pulldaya method in dalobject and checks if that is not a null 
                        Console.WriteLine("please enter a number from the menue ");
                        SysFunc.printEnum<Details>();
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Details>((Details)enter));
                        switch (enter)
                        {
                            case (int)Details.BaseStaion:
                                Console.WriteLine("enter Station Id please : ");
                                enter = SysFunc.SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataStation(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataStation(enter)));
                                break;
                            case (int)Details.Costumer:
                                Console.WriteLine("enter Costumer Id please : ");
                                enter = SysFunc.SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataCostumer(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataCostumer(enter)));
                                break;
                            case (int)Details.Drone:
                                Console.WriteLine("enter Drone Id please : ");
                                enter = SysFunc.SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataDrone(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataDrone(enter)));
                                break;
                            case (int)Details.Package:
                                Console.WriteLine("enter Parcel Id please : ");
                                enter = SysFunc.SafeEnterUInt();
                                Console.WriteLine((DalObject.PullDataParcel(enter).Equals(null) ? "No Object found with that ID please try again sir" : DalObject.PullDataParcel(enter)));
                                break;

                        }
                        break;
                    case (int)Menu.Update:
                        ///just calling the dalobject methods 
                        Console.WriteLine("please enter a number from the menue ");
                        SysFunc.printEnum<Update>();
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Update>((Update)enter));
                        switch (enter)
                        {
                            case (int)Update.PackgeandDrone:
                                Console.WriteLine(DalObject.BindParcelToDrone(SysFunc.SafeEnterUInt("enter drone id: "),SysFunc.SafeEnterUInt("Parcel Id: "), SysFunc.SafeEnterUInt("Target Id: "), SysFunc.SafeEnterUInt("Sender Id: ")));
                   
                                break;

                            case (int)Update.PackgeTakeDrone:
                                DalObject.PickUpByDrone(SysFunc.SafeEnterUInt("enter the Id of the package you want to pickup: "));
                                break;
                            case (int)Update.PackgeTakeCostumer:
                                Console.WriteLine("Enter the package  ID please : ");
                                DalObject.ParcelDeliveredToCostumer(SysFunc.SafeEnterUInt());

                                break;
                            case (int)Update.DroneSendCharge:
                                Console.WriteLine("Enter the Drone  ID and then Station  ID please : ");
                                DalObject.DroneCharge(SysFunc.SafeEnterUInt(), SysFunc.SafeEnterUInt());

                                break;
                            case (int)Update.DroneRelease:
                                Console.WriteLine("Enter the Drone ID please : ");
                                DalObject.DroneChargeRealse(SysFunc.SafeEnterUInt());
                                break;

                        }
                        break;
                    case (int)Menu.ListShow:
                        /// get the list from daloject method anmd use sysfunc function to print it 
                        Console.WriteLine("please enter a number from the menue ");
                        SysFunc.printEnum<ListShow>();
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
                               SysFunc.printList<Parcel>( DalObject.ParcelsWithoutDronesPrint());
                                
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
/*
 
 
 
 EEEEEEEEEEEEEEEEEEEEEENNNNNNNN        NNNNNNNNDDDDDDDDDDDDD
E::::::::::::::::::::EN:::::::N       N::::::ND::::::::::::DDD
E::::::::::::::::::::EN::::::::N      N::::::ND:::::::::::::::DD
EE::::::EEEEEEEEE::::EN:::::::::N     N::::::NDDD:::::DDDDD:::::D
  E:::::E       EEEEEEN::::::::::N    N::::::N  D:::::D    D:::::D
  E:::::E             N:::::::::::N   N::::::N  D:::::D     D:::::D
  E::::::EEEEEEEEEE   N:::::::N::::N  N::::::N  D:::::D     D:::::D
  E:::::::::::::::E   N::::::N N::::N N::::::N  D:::::D     D:::::D
  E:::::::::::::::E   N::::::N  N::::N:::::::N  D:::::D     D:::::D
  E::::::EEEEEEEEEE   N::::::N   N:::::::::::N  D:::::D     D:::::D
  E:::::E             N::::::N    N::::::::::N  D:::::D     D:::::D
  E:::::E       EEEEEEN::::::N     N:::::::::N  D:::::D    D:::::D
EE::::::EEEEEEEE:::::EN::::::N      N::::::::NDDD:::::DDDDD:::::D
E::::::::::::::::::::EN::::::N       N:::::::ND:::::::::::::::DD
E::::::::::::::::::::EN::::::N        N::::::ND::::::::::::DDD
EEEEEEEEEEEEEEEEEEEEEENNNNNNNN         NNNNNNNDDDDDDDDDDDDD



 */

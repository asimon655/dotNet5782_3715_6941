using System;
using BL; 
namespace ConsoleUI_BL
{
    class Program
    {
        static private Bl Logistics = new Bl();
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
                        /// in all the adds i just create dumy struct and fill it with vals and call the add method in Logistics
                        SysFunc.printEnum<Add>();
                        enter = SysFunc.SafeEnterUInt("please enter a number from the menue ");
                        Console.WriteLine(EnumHelper.GetDescription<Add>((Add)enter));
                        switch (enter)
                        {
                            case (int)Add.Costumer:
                                {

                                    int Id = SysFunc.SafeEnterUInt("Id: ");
                                    double Lattitude = SysFunc.SafeEnterDouble("Lattitude: ");
                                    double Longitude = SysFunc.SafeEnterDouble("Longitude: ");
                                    Console.Write("Name: ");
                                    String Name = Console.ReadLine();
                                    Console.Write("Phone: ");
                                    String Phone = Console.ReadLine();
                                    try
                                    {
                                        Logistics.AddCostumer(Id, Name, Phone , Longitude,Lattitude);
                                        Console.WriteLine("The costumer added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine(err.Message);
                                    }

                                }
                                break;
                            case (int)Add.Drone:
                                {
                                    int Id = SysFunc.SafeEnterUInt("Id: ");
                                    double Battery = SysFunc.SafeEnterDouble("Battery: ");
                                    Console.WriteLine("Maxweight=>please enter a number from the menue : ");
                                    SysFunc.printEnum<WeightCategories>();
                                    WeightCategories MaxWeigth = (WeightCategories)SysFunc.SafeEnterUInt("Maxweight: (Choose from the numbers above:  "); ;
                                    Console.Write("Model: ");
                                    String Model = Console.ReadLine();
                                    DroneStatuses Status = (DroneStatuses)DroneStatuses.Free;
                                    try
                                    {
                                        Logistics.AddDrone(Id, Model,MaxWeigth, Status ,Battery);
                                        Console.WriteLine("The costumer added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine(err.Message);
                                    }
                               
                                }
                                break;
                            case (int)Add.Package:
                                {
                                    Console.WriteLine("Priorty=>please enter a number from the menue : ");
                                    SysFunc.printEnum<Priorities>();
                                    Console.WriteLine(EnumHelper.GetDescription<Priorities>((Priorities)enter));
                                    Priorities Priority = (Priorities)SysFunc.SafeEnterUInt("enter a number from the numbers above");
                                    Console.WriteLine("weight=>please enter a number from the menue : ");
                                    SysFunc.printEnum<WeightCategories>();
                                    WeightCategories Weight = (WeightCategories)SysFunc.SafeEnterUInt("enter a number from the numbers above : "); ;
                                    try
                                    {
                                        Logistics.AddPackage( Weight,Priority);
                                        Console.WriteLine("The costumer added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine(err.Message);
                                    }
                                } 
                                break;
                            case (int)Add.Staion:
                                {
                                    int Id = SysFunc.SafeEnterUInt("Id: ");
                                    int Name = SysFunc.SafeEnterUInt("Name: ");
                                    double Latitude = SysFunc.SafeEnterDouble("Latitude: ");
                                    double Longitude = SysFunc.SafeEnterDouble("Longitude: ");
                                    int ChargeSlots = SysFunc.SafeEnterUInt("ChargeSlots: ");
                                    try
                                    {
                                        Logistics.AddStaion(Id , Name , Latitude ,Longitude , ChargeSlots);
                                        Console.WriteLine("The costumer added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine(err.Message);
                                    }
                                }
                                break;

                        }
                        break;
                    case (int)Menu.Details:
                        /// in all the details i just get a drone id and call the pulldaya method in Logistics and checks if that is not a null 
                        Console.WriteLine("please enter a number from the menue ");
                        SysFunc.printEnum<Details>();
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Details>((Details)enter));
                        switch (enter)
                        {
                            case (int)Details.BaseStaion:
                                Console.WriteLine("enter Station Id please : ");
                              
                                try
                                {
                                    
                                    String StaionData = Logistics.PullDataStaion(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(StaionData);

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                } 
                                break;
                            case (int)Details.Costumer:
                                Console.WriteLine("enter Costumer Id please : ");
                                try
                                {

                                    String StaionData = Logistics.PullDataCostumer(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(StaionData);

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                            case (int)Details.Drone:
                                Console.WriteLine("enter Drone Id please : ");
                                try
                                {

                                    String StaionData = Logistics.PullDataDrone(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(StaionData);

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                            case (int)Details.Package:
                                Console.WriteLine("enter Parcel Id please : ");
                                try
                                {

                                    String StaionData = Logistics.PullDataParcel(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(StaionData);

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;

                        }
                        break;
                    case (int)Menu.Update:
                        ///just calling the Logistics methods 
                        Console.WriteLine("please enter a number from the menue ");
                        SysFunc.printEnum<Update>();
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Update>((Update)enter));
                        switch (enter)
                        {
                            case (int)Update.PackgeandDrone:
                                try
                                {

                                    Logistics.BindParcelToDrone(SysFunc.SafeEnterUInt("enter drone id: "), SysFunc.SafeEnterUInt("Parcel Id: "), SysFunc.SafeEnterUInt("Target Id: "), SysFunc.SafeEnterUInt("Sender Id: "));
                                    Console.WriteLine("Binded succefuly ");

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                                

                               

                            case (int)Update.PackgeTakeDrone:
                                try
                                {

                                    Logistics.PickUpByDrone(SysFunc.SafeEnterUInt("enter the Id of the package you want to pickup: "));
                                    Console.WriteLine("Package has Taken by Drone succefuly  ");

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                               
                              
                            case (int)Update.PackgeTakeCostumer:
                                try
                                {

                                    Logistics.ParcelDeliveredToCostumer(SysFunc.SafeEnterUInt("Enter the package  ID please : "));
                                    Console.WriteLine("Package has Taken by Drone succefuly  ");

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                            case (int)Update.DroneSendCharge:
                              
                                try
                                {

                                    Logistics.DroneCharge(SysFunc.SafeEnterUInt("Drone  ID: "), SysFunc.SafeEnterUInt(" Station  ID : "));
                                    Console.WriteLine("Drone has sent to Charging port succefuly ");

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                            case (int)Update.DroneRelease:
                                try
                                {

                                    Logistics.DroneChargeRealse(SysFunc.SafeEnterUInt("Drone ID: " ));
                                    Console.WriteLine("Drone has sent to Charging port succefuly ");

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine(err.Message);
                                }
                                break;
                             

                        }
                        break;
                    case (int)Menu.ListShow:
                        /// get the list from Logistics method anmd use sysfunc function to print it 
                        Console.WriteLine("please enter a number from the menue ");
                        SysFunc.printEnum<ListShow>();
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<ListShow>((ListShow)enter));
                        switch (enter)
                        {
                            case (int)ListShow.BaseStaions:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<String>(Logistics.StaionsPrint());
                                break;
                            case (int)ListShow.BaseStaionsFreePorts:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<String>(Logistics.BaseStaionsFreePortsPrint());
                                break;
                            case (int)ListShow.Costumers:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<String>(Logistics.CostumersPrint());
                                break;
                            case (int)ListShow.Drones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<Drone>(Logistics.DronesPrint());
                                break;
                            case (int)ListShow.Packages:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<String>(Logistics.ParcelsPrint());
                                break;
                            case (int)ListShow.PackagesWithoutDrones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<String>(Logistics.ParcelsWithoutDronesPrint());

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

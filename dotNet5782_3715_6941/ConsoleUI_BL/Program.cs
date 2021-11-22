using System;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {
        static private IBL.Ibl Logistics;
        static void Main(string[] args)
        {
            Logistics = new BL.Bl();
             
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
                                    Costumer costumer = new Costumer();
                                    costumer.Id = SysFunc.SafeEnterUInt("Id: ");
                                    costumer.Loct = new Location(SysFunc.SafeEnterDouble("Longitude: "), SysFunc.SafeEnterDouble("Lattitude: ")); 
                                    Console.Write("Name: ");
                                    costumer.Name = Console.ReadLine();
                                    Console.Write("Phone: ");
                                    costumer.Phone_Num = Console.ReadLine();
                                    try
                                    {
                                        Logistics.AddCostumer(costumer);
                                        Console.WriteLine("The costumer added succefully ");
                                    }
                                    catch (IdAlreadyExists err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
                                    catch (LocationOutOfRange err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
           
                                }
                                break;
                            case (int)Add.Drone:
                                {
                                    Drone drone = new Drone();
                                    drone.Id = SysFunc.SafeEnterUInt("Id: ");
                                    Console.WriteLine("Maxweight=>please enter a number from the menue : ");
                                    SysFunc.printEnum<WeightCategories>();
                                    WeightCategories MaxWeigth = (WeightCategories)SysFunc.SafeEnterUInt("Maxweight: (Choose from the numbers above:  "); ;
                                    Console.Write("Model: ");
                                    drone.Model = Console.ReadLine();
                                    int stationId = SysFunc.SafeEnterUInt("enter the Station Id to put the drone in : ");

                                    try
                                    {
                                        Logistics.AddDrone(drone, stationId);
                                        Console.WriteLine("The drone added succefully ");
                                    }
                                    catch (IdAlreadyExists err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
                                    catch (EnumOutOfRange err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    } 
                               
                                }
                                break;
                            case (int)Add.Package:
                                {
                                    Parcel parcel = new Parcel();
                                    parcel.SenderParcelToCostumer = new ParcelToCostumer() { id = SysFunc.SafeEnterUInt("please enter Sender ID: ") };
                                    parcel.GetterParcelToCostumer = new ParcelToCostumer() { id = SysFunc.SafeEnterUInt("please enter Target ID: ") };
                                    Console.WriteLine("Priorty=>please enter a number from the menue : ");
                                    SysFunc.printEnum<Priorities>();
                                    Console.WriteLine(EnumHelper.GetDescription<Priorities>((Priorities)enter));
                                    parcel.Priority = (IBL.BO.Priorities)SysFunc.SafeEnterUInt("enter a number from the numbers above");
                                    Console.WriteLine("weight=>please enter a number from the menue : ");
                                    SysFunc.printEnum<WeightCategories>();
                                    parcel.Weight = (IBL.BO.WeightCategories)SysFunc.SafeEnterUInt("enter a number from the numbers above : "); ;
                                    try
                                    {
                                        Logistics.AddParcel(parcel);
                                        Console.WriteLine("The parcel added succefully ");
                                    }
                                    catch (IdAlreadyExists err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
                                    catch (EnumOutOfRange err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }

                                }
                                break;
                            case (int)Add.Staion:
                                {
                                    BaseStation station = new BaseStation();
                                    station.Id = SysFunc.SafeEnterUInt("Id: ");
                                    station.Name = SysFunc.SafeEnterUInt("Name: ");
                                    station.LoctConstant = new Location (SysFunc.SafeEnterDouble("Latitude: "), SysFunc.SafeEnterDouble("Longitude: "));
                                    station.NumOfFreeOnes = SysFunc.SafeEnterUInt("ChargeSlots: ");
                                    try
                                    {
                                        Logistics.AddStation(station);
                                        Console.WriteLine("The station added succefully ");
                                    }
                                    catch (IdAlreadyExists err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
                                    catch (LocationOutOfRange err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }

                                }
                                break;
                            case (int)ListShow.exit:
                                break;
                        }
                        break;
                    case (int)Menu.Details:
                        /// in all the details i just get a drone id and call the pulldaya method in Logistics and checks if that is not a null 
                        SysFunc.printEnum<Details>();
                        Console.WriteLine("please enter a number from the menue ");
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Details>((Details)enter));
                        switch (enter)
                        {
                            case (int)Details.BaseStaion:
                                Console.WriteLine("enter Station Id please : ");
                                try
                                {
                                    BaseStation station = Logistics.PullDataStaion(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(station.ToString());
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                } 
                                break;
                            case (int)Details.Costumer:
                                Console.WriteLine("enter Costumer Id please : ");
                                try
                                {
                                    Costumer costumer = Logistics.PullDataCostumer(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(costumer.ToString());
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Details.Drone:
                                Console.WriteLine("enter Drone Id please : ");
                                try
                                {

                                    Drone drone = Logistics.PullDataDrone(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(drone.ToString());

                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Details.Package:
                                Console.WriteLine("enter Parcel Id please : ");
                                try
                                {

                                    Parcel parcel = Logistics.PullDataParcel(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(parcel.ToString());

                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Details.exit:
                                break;
                        }
                        break;
                    case (int)Menu.Update:
                        ///just calling the Logistics methods 
                        SysFunc.printEnum<Update>();
                        Console.WriteLine("please enter a number from the menue ");
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Update>((Update)enter));
                        switch (enter)
                        {
                            case (int)Update.UpdateDrone:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter Drone Id : ");
                                    Console.WriteLine("enter Drone modle Name : ");
                                    string name = Console.ReadLine();
                                    Logistics.UpdateDrone(id, name);

                                    Console.WriteLine("Updated succefuly ");
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Update.UpdateStation:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter Station Id : ");
                                    int name = SysFunc.SafeEnterUInt("enter Station Name : ");
                                    int chargeSlots = SysFunc.SafeEnterUInt("enter Station Charge Slots : ");
                                    Logistics.UpdateStation(id, name, chargeSlots);

                                    Console.WriteLine("Updated succefuly ");
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Update.UpdateCostumer:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter Costumer Id : ");
                                    Console.WriteLine("enter Costumer Name : ");
                                    string name = Console.ReadLine();
                                    Console.WriteLine("enter Costumer Phone number : ");
                                    string phone = Console.ReadLine();
                                    Logistics.UpdateCostumer(id, name, phone);

                                    Console.WriteLine("Updated succefuly ");
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Update.BindPackgeAndDrone:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter drone id: ");
                                    Logistics.BindParcelToDrone(id);

                                    Console.WriteLine("Binded succefuly ");
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                break;
                            case (int)Update.DroneTakePackge:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter drone id: ");
                                    Logistics.PickUpByDrone(id);

                                    Console.WriteLine("Package has Taken by Drone succefuly  ");
                                }
                                catch (EnumNotInRightStatus<DroneStatuses> err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                catch (EnumNotInRightStatus<ParcelStat> err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (CantReachToDest err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;  
                            case (int)Update.DroneProvidePackge:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter drone id : ");
                                    Logistics.ParcelDeliveredToCostumer(id);

                                    Console.WriteLine("Package has Taken by Drone succefuly  ");
                                }
                                catch (EnumNotInRightStatus<ParcelStat> err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                catch (CantReachToDest err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.DroneSendCharge:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("Drone  ID: ");
                                    Logistics.DroneCharge(id);

                                    Console.WriteLine("Drone has sent to Charging port succefuly ");
                                }
                                catch (EnumNotInRightStatus<DroneStatuses> err)
                                {
                                    Console.WriteLine("Error : "+err);
                                }
                                catch (CantReachToDest err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (IdAlreadyExists err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.DroneRelease:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("Drone ID: ");
                                    double chargingPeriod = SysFunc.SafeEnterDouble("enter how much time the drone was in charge : ");
                                    Logistics.DroneChargeRelease(id, chargingPeriod);

                                    Console.WriteLine("Drone has released from Charging port succefuly ");
                                }
                                catch (EnumNotInRightStatus<DroneStatuses> err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (CantReachToDest err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                catch (IdDosntExists err)
                                {
                                    Console.WriteLine("Error : " + err);
                                } 
                                break;
                            case (int)Update.exit:
                                break;
                        }
                        break;
                    case (int)Menu.ListShow:
                        /// get the list from Logistics method anmd use sysfunc function to print it 
                        SysFunc.printEnum<ListShow>();
                        Console.WriteLine("please enter a number from the menue ");
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<ListShow>((ListShow)enter));
                        switch (enter)
                        {
                            case (int)ListShow.BaseStaions:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<BaseStaionToList>(Logistics.StaionsPrint());
                                break;
                            case (int)ListShow.BaseStaionsFreePorts:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<BaseStaionToList>(Logistics.BaseStaionsFreePortsPrint());
                                break;
                            case (int)ListShow.Costumers:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<ClientToList>(Logistics.CostumersPrint());
                                break;
                            case (int)ListShow.Drones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<DroneToList>(Logistics.DronesPrint());
                                break;
                            case (int)ListShow.Packages:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<ParcelToList>(Logistics.ParcelsPrint());
                                break;
                            case (int)ListShow.PackagesWithoutDrones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<ParcelToList>(Logistics.ParcelsWithoutDronesPrint());
                                break;
                            case (int)ListShow.exit:
                                break;
                        }
                        break;
                    case (int)Menu.exit:
                        break;

                }
            } while ((command != (int)Menu.exit));

        }
    }
    
}

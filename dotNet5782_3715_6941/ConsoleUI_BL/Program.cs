﻿using BO;
using System;

namespace ConsoleUI_BL
{
    internal class Program
    {
        private static BlApi.Ibl Logistics;

        private static void Main(string[] args)
        {
            Logistics = BlApi.BlFactory.GetBl();

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
                                    Customer costumer = new Customer
                                    {
                                        Id = SysFunc.SafeEnterUInt("Id: "),
                                        Loct = new Location(SysFunc.SafeEnterDouble("Longitude: "), SysFunc.SafeEnterDouble("Lattitude: "))
                                    };
                                    Console.Write("Name: ");
                                    costumer.Name = Console.ReadLine();
                                    Console.Write("Phone: ");
                                    costumer.Phone_Num = Console.ReadLine();
                                    try
                                    {
                                        Logistics.AddCustomer(costumer);
                                        Console.WriteLine("The costumer added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }

                                }
                                break;
                            case (int)Add.Drone:
                                {
                                    Drone drone = new Drone
                                    {
                                        Id = SysFunc.SafeEnterUInt("Id: ")
                                    };
                                    Console.WriteLine("Maxweight=>please enter a number from the menue : ");
                                    SysFunc.printEnum<BO.WeightCategories>();
                                    drone.Weight = (BO.WeightCategories)SysFunc.SafeEnterUInt("Maxweight: (Choose from the numbers above:  "); ;
                                    Console.Write("Model: ");
                                    drone.Model = Console.ReadLine();
                                    int stationId = SysFunc.SafeEnterUInt("enter the Station Id to put the drone in : ");

                                    try
                                    {
                                        Logistics.AddDrone(drone, stationId);
                                        Console.WriteLine("The drone added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
                                }
                                break;
                            case (int)Add.Package:
                                {
                                    Parcel parcel = new Parcel
                                    {
                                        SenderParcelToCostumer = new CustomerInParcel() { id = SysFunc.SafeEnterUInt("please enter Sender ID: ") },
                                        GetterParcelToCostumer = new CustomerInParcel() { id = SysFunc.SafeEnterUInt("please enter Target ID: ") }
                                    };
                                    Console.WriteLine("Priorty=>please enter a number from the menue : ");
                                    SysFunc.printEnum<Priorities>();
                                    parcel.Priority = (BO.Priorities)SysFunc.SafeEnterUInt("enter a number from the numbers above");
                                    Console.WriteLine("weight=>please enter a number from the menue : ");
                                    SysFunc.printEnum<WeightCategories>();
                                    parcel.Weight = (BO.WeightCategories)SysFunc.SafeEnterUInt("enter a number from the numbers above : "); ;
                                    try
                                    {
                                        Logistics.AddParcel(parcel);
                                        Console.WriteLine("The parcel added succefully ");
                                    }
                                    catch (Exception err)
                                    {
                                        Console.WriteLine("Error : " + err);
                                    }
                                }
                                break;
                            case (int)Add.Staion:
                                {
                                    Station station = new Station
                                    {
                                        Id = SysFunc.SafeEnterUInt("Id: ")
                                    };
                                    Console.WriteLine("Name: ");
                                    station.Name = Console.ReadLine();
                                    station.LoctConstant = new Location(SysFunc.SafeEnterDouble("Latitude: "), SysFunc.SafeEnterDouble("Longitude: "));
                                    station.NumOfFreeOnes = SysFunc.SafeEnterUInt("ChargeSlots: ");
                                    try
                                    {
                                        Logistics.AddStation(station);
                                        Console.WriteLine("The station added succefully ");
                                    }
                                    catch (Exception err)
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
                        /// in all the details i just get a drone id and call the pulldata method in Logistics
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
                                    Station station = Logistics.GetStation(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(station.ToString());
                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Details.Costumer:
                                Console.WriteLine("enter Costumer Id please : ");
                                try
                                {
                                    Customer costumer = Logistics.GetCostumer(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(costumer.ToString());
                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Details.Drone:
                                Console.WriteLine("enter Drone Id please : ");
                                try
                                {

                                    Drone drone = Logistics.GetDrone(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(drone.ToString());

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Details.Package:
                                Console.WriteLine("enter Parcel Id please : ");
                                try
                                {

                                    Parcel parcel = Logistics.GetParcel(SysFunc.SafeEnterUInt());
                                    Console.WriteLine(parcel.ToString());

                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
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
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.UpdateStation:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter Station Id : ");
                                    Console.WriteLine("enter Station Name : ");
                                    string name = Console.ReadLine();
                                    int chargeSlots = SysFunc.SafeEnterUInt("enter Station Charge Slots : ");
                                    Logistics.UpdateStation(id, name, chargeSlots);

                                    Console.WriteLine("Updated succefuly ");
                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
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
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.BindPackgeAndDrone:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter drone id: ");
                                    Logistics.BindParcelToDrone(id);

                                    Console.WriteLine("Binded succefuly ");
                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.DroneTakePackge:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter drone id: ");
                                    Logistics.DronePickUp(id);

                                    Console.WriteLine("Package has beem Taken by Drone succefuly  ");
                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.DroneProvidePackge:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("enter drone id : ");
                                    Logistics.DroneDelivere(id);

                                    Console.WriteLine("Package has been delivered by Drone succefuly  ");
                                }
                                catch (Exception err)
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
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Update.DroneRelease:
                                try
                                {
                                    int id = SysFunc.SafeEnterUInt("Drone ID: ");
                                    double chargingPeriod = SysFunc.SafeEnterDouble("enter how much time the drone was in charge : ");
                                    Logistics.DroneReleaseCharge(id, chargingPeriod);

                                    Console.WriteLine("Drone has released from Charging port succefuly ");
                                }
                                catch (Exception err)
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
                                SysFunc.printList<StationList>(Logistics.GetStations());
                                break;
                            case (int)ListShow.BaseStaionsFreePorts:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<StationList>(Logistics.GetStationsWithFreePorts());
                                break;
                            case (int)ListShow.Costumers:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<CustomerList>(Logistics.GetCustomers());
                                break;
                            case (int)ListShow.Drones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<DroneList>(Logistics.GetDrones());
                                break;
                            case (int)ListShow.Packages:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<ParcelList>(Logistics.GetParcels());
                                break;
                            case (int)ListShow.PackagesWithoutDrones:
                                Console.WriteLine("Printing ... ");
                                SysFunc.printList<ParcelList>(Logistics.GetUnbindedParcels());
                                break;
                            case (int)ListShow.exit:
                                break;
                        }
                        break;
                    case (int)Menu.Delete:
                        /// get the list from Logistics method anmd use sysfunc function to print it 
                        SysFunc.printEnum<Delete>();
                        Console.WriteLine("please enter a number from the menue ");
                        Console.WriteLine((int.TryParse(Console.ReadLine(), out enter) ? "" : "please enter only numbers"));
                        Console.WriteLine(EnumHelper.GetDescription<Delete>((Delete)enter));
                        switch (enter)
                        {
                            case (int)Delete.Parcel:
                                Console.WriteLine("enter Parcel Id please : ");
                                try
                                {
                                    Logistics.DeleteParcel(SysFunc.SafeEnterUInt());
                                    Console.WriteLine("the deletion operation went succsfully");
                                }
                                catch (Exception err)
                                {
                                    Console.WriteLine("Error : " + err);
                                }
                                break;
                            case (int)Delete.exit:
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

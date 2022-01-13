using System.ComponentModel;

namespace ConsoleUI_BL
{
    // the Main Menu options
    public enum Menu
    {
        [Description("Exit")]
        exit,
        [Description("Add")]
        Add,
        [Description("Update")]
        Update,
        [Description("Show status and details ")]
        Details,
        [Description("Show lists of objects")]
        ListShow,
        [Description("Delete")]
        Delete
    }
    // the options of the option Add (Add from the main menu)
    public enum Add
    {
        [Description("Exit")]
        exit,
        [Description("Add Station for the Staions List")]
        Staion,
        [Description("Add Drone to the Drone List ")]
        Drone,
        [Description("Add new costumer ")]
        Costumer,
        [Description("Add Package")]
        Package,
    }
    // the options of the option Update (Update from the main menu)
    public enum Update
    {
        [Description("Exit")]
        exit,
        [Description("Update drone model name")]
        UpdateDrone,
        [Description("Update station name and charging slots")]
        UpdateStation,
        [Description("Update costumer name and phone")]
        UpdateCostumer,
        [Description("Bind packge and drone")]
        BindPackgeAndDrone,
        [Description("Update- Drone picks up a packge ")]
        DroneTakePackge,
        [Description("Update Costumer picking up a package ")]
        DroneProvidePackge,
        [Description("Update -send Drone to base staion (charge)")]
        DroneSendCharge,
        [Description("Update -release Drone from base staion ")]
        DroneRelease,
    }
    // the options of the option Details (Details from the main menu)
    public enum Details
    {
        [Description("Exit")]
        exit,
        [Description("Show status and details of basestaion ")]
        BaseStaion,
        [Description("Show status and details of drone ")]
        Drone,
        [Description("Show status and details of Costumer")]
        Costumer,
        [Description("Show status and details of Package ")]
        Package,
    }
    // the options of the option ListShow (ListShow from the main menu)
    public enum ListShow
    {
        [Description("Exit")]
        exit,
        [Description("Show lists of Base Staions")]
        BaseStaions,
        [Description("Show lists of Drones")]
        Drones,
        [Description("Show lists of Costumers")]
        Costumers,
        [Description("Show lists of Parcels")]
        Packages,
        [Description("Show lists of Packages without Drone")]
        PackagesWithoutDrones,
        [Description("Show lists of of Base Staions with free charge ports ")]
        BaseStaionsFreePorts,
    }
    public enum Delete
    {
        [Description("Exit")]
        exit,
        [Description("Delete Parcel")]
        Parcel
    }
    public enum Priorities
    {
        [Description("Regular delivery ")]
        Regular,
        [Description("fast delivery")]
        Fast,
        [Description("Emergency delivery ")]
        Emergency
        ,
    }
    /// <summary>
    /// all the enum names explains the self 
    /// i used description because i can reuse it in the gui and it look like the coolest thing 
    /// </summary>
    /// 
    public enum WeightCategories
    {
        [Description("easy weight ")]
        Easy,
        [Description("medium weight ")]
        Medium,
        [Description("heavy weight  ")]
        Heavy,
    };
}
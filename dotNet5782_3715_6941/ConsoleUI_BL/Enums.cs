using System;

namespace ConsoleUI_BL
{
    
        // the Main Menu options
        public enum Menu
        {
            [Description("Add")]
            Add,
            [Description("Update")]
            Update,
            [Description("Show status and details ")]
            Details,
            [Description("Show lists of objects")]
            ListShow,
            [Description("Exit")]
            exit 



        }
        // the options of the option Add (Add from the main menu)
        public enum Add { 
        
            [Description("Add Station for the Staions List")]
            Staion ,
            [Description("Add Drone to the Drone List ")]
            Drone,
            [Description("Add new costumer ")]
            Costumer ,
            [Description("Add Package")]
            Package, 
        }
        // the options of the option Update (Update from the main menu)
        public enum Update 
        {

            [Description("Bind packge and drone")]
            PackgeandDrone,
            [Description("Update- Drone picks up a packge ")]
            PackgeTakeDrone,
            [Description("Update Costumer picking up a package ")]
            PackgeTakeCostumer,
            [Description("Update -send Drone to base staion (charge)")]
            DroneSendCharge,
            [Description("Update -release Drone from base staion ")]
            DroneRelease,


        }
        // the options of the option Details (Details from the main menu)
        public enum Details
        {
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
}
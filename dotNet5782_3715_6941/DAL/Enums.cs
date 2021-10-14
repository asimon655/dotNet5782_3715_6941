﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
/*  
 ___ ____  _   _ __  __
|_ _|  _ \| | | |  \/  |
 | || | | | |_| | |\/| |
 | || |_| |  _  | |  | |
|___|____/|_| |_|_|  |_|
 */
namespace IDAL
{
    namespace DO
    {
       public  enum WeightCategories
        {
            [Description("easy weight ")]
            Easy ,
            [Description("medium weight ")]
            Medium,
            [Description("heavy weight  ")]
            Heavy,


        };
        public   enum DroneStatuses
        {
            [Description("free Drone  ")]
            Free,
            [Description("Drone is in Delivery ")]
            Delivery,
            [Description("Drone need Matance  ")]
            Matance ,
         



        };
        public enum Priorities 
        {
            [Description("Regular delivery ")]
            Regular,
            [Description("fast delivery")]
            Fast ,
            [Description("Emergency delivery ")]
            Emergency 
            ,
             
        
        }
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
            [Description("Exiting ... Please give as 100 ;)")]
            exit =-1



        }
        public enum Add { 
        
            [Description("Add Staion for the Staions List")]
            Staion ,
            [Description("Add Drone to the Drone List ")]
            Drone,
            [Description("Add new costumer ")]
            Costumer ,
            [Description("Add Package")]
            Package, 



    }
        public enum Update 
        {

            [Description("Bind packge and drone")]
            PackgeandDrone,
            [Description("Update- Drone pick a packge ")]
            PackgeTakeDrone,
            [Description("Update Costumer pick a package ")]
            PackgeTakeCostumer,
            [Description("Update -send Drone to base staion ")]
            PackgeSend,


        }
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
 

}

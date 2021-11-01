﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
/*
 
    _/_/_/  _/_/_/    _/    _/  _/      _/
     _/    _/    _/  _/    _/  _/_/  _/_/
    _/    _/    _/  _/_/_/_/  _/  _/  _/
   _/    _/    _/  _/    _/  _/      _/
_/_/_/  _/_/_/    _/    _/  _/      _/


 
 
       _/_/              _/
   _/    _/    _/_/_/      _/_/_/  _/_/      _/_/    _/_/_/
  _/_/_/_/  _/_/      _/  _/    _/    _/  _/    _/  _/    _/
 _/    _/      _/_/  _/  _/    _/    _/  _/    _/  _/    _/
_/    _/  _/_/_/    _/  _/    _/    _/    _/_/    _/    _/


 
 
 */
namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// all the enum names explains the self 
        /// i used description because i can reuse it in the gui and it look like the coolest thing 
        /// </summary>
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
        // the Main Menu options
       
    }
}

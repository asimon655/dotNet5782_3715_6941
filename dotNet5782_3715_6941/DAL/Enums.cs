using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace IDAL
{
    namespace DO
    {
        enum WeightCategories
        {
            [Description("easy weight ")]
            Easy ,
            [Description("medium weight ")]
            Medium,
            [Description("heavy weight  ")]
            Heavy,


        };
        enum DroneStatuses
        {
            [Description("free Drone  ")]
            Free,
            [Description("Drone need Matance  ")]
            Matance ,
            [Description("Drone is in Delivery " )]
            Delivery , 



        };
        enum Priorities 
        {
            [Description("Regular delivery ")]
            Regular,
            [Description("fast delivery")]
            Fast ,
            [Description("Emergency delivery ")]
            Emergency 
            ,
             
        
        } 
    
    
    } 
}

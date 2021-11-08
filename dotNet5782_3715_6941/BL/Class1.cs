using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace BL
{

    /// <summary>
    /// all the enum names explains the self 
    /// i used description because i can reuse it in the gui and it look like the coolest thing 
    /// </summary>
    public enum WeightCategories
    {
        [Description("easy weight ")]
        Easy,
        [Description("medium weight ")]
        Medium,
        [Description("heavy weight  ")]
        Heavy,
    };

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
}

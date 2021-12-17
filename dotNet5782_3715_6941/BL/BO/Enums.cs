using System.ComponentModel;

namespace BO
{
    public enum Priorities
    {
        [Description("Regular delivery ")]
        Regular,
        [Description("fast delivery")]
        Fast,
        [Description("Emergency delivery ")]
        Emergency,
    }
    /// <summary>
    /// all the enum names explains the self 
    /// i used description because i can reuse it in the gui and it look like the coolest thing 
    /// </summary>
    /// 
    public  enum WeightCategories
    {
        [Description("easy weight ")]
        Easy,
        [Description("medium weight ")]
        Medium,
        [Description("heavy weight  ")]
        Heavy,
    };
    public  enum ParcelStatus
    {
        [Description("Parcel Declared ")]
        Declared ,
        [Description("Parcel Binded ")]
        Binded,
        [Description("Parcel PickedUp  ")]
        PickedUp,
        [Description("Parcel  Delivered")]
        Delivered,



    }
    public enum DroneStatuses
    {
        [Description("Drone is in Delivery ")]
        Delivery,
        [Description("free Drone  ")]
        Free,  
        [Description("Drone need Matance  ")]
        Matance,
     
    };
}

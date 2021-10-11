using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        enum WeightCategories { };
        enum Priorities { };
        struct Costumer
        {
            int Id;
            string Name;
            string Phone;
            double Longitude;
            double Lattitude;
        }

        struct Parcel
        {
            int Id;
            int SenderId;
            int TargetId;
            WeightCategories Weight;
            Priorities Priority;
            DateTime Requested;
            int DroneId;
            DateTime Schedulded;
            DateTime PickedUp;
            DateTime Delivered;
        }
    }
}

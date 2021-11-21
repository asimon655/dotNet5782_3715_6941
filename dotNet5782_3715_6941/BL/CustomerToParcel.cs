using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    {
        public class CustomerToParcel : Printable
        {
            public int Id { set; get; }
            public WeightCategories Weight {set; get ; }
            public Priorities Priority { set; get;  } 
            public ParcelStat Status { set; get; }
            public ParcelToCostumer ParentCustomer { set; get; }
        }

    }
}
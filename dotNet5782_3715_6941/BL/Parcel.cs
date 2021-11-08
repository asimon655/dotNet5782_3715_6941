using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Parcel
        {
            public int Id { set; get; }
            public IBL.BO.ParcelToCostumer SenderParcelToCostumer { set; get; }
            public IBL.BO.ParcelToCostumer GetterParcelToCostumer { set; get; }

            public WeightCategories Weight {set; get ; }
            public Priorities Priority { set; get;  } 
            public ParcelToDrone ParcelDrone { set; get;  }
            public DateTime ParcelCreation { set; get;  } 
            public DateTime ParcelBinded { set; get;  }
            public DateTime ParcelPickedUp { set; get; }
            public DateTime ParcelDelivered { set; get;  }

        }
    } 
}

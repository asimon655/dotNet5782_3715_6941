using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInTransfer : OverrideToString
        {
            public int Id { set; get; }
            public Priorities Priorety{ set; get;  }
            public WeightCategories Weight { set; get;  }
            public IBL.BO.ParcelToCostumer Sender { set; get;  }
            public IBL.BO.ParcelToCostumer Target { set; get; }
            public Location Pickup { set; get;  }
            public Location Dst { set; get;  }
            public double Distance { set; get;  }

            

        }
    }
}

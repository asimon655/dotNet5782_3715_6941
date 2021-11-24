using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInTransfer 
        {
            public int Id { set; get; }
            public Priorities Priorety{ set; get;  }
            public WeightCategories Weight { set; get;  }
            public IBL.BO.ParcelToCostumer Sender { set; get;  }
            public IBL.BO.ParcelToCostumer Target { set; get; }
            public Location Pickup { set; get;  }
            public Location Dst { set; get;  }
            public double Distance { set; get;  }

            public override string ToString()
            {
                return $"Id : {Id}\n" +
                       $"Priorety : {Priorety}\n" +
                       $"Weight : {Weight}\n" +
                       $"Sender : {Sender}\n" +
                       $"Target : {Target}\n" +
                       $"Sender location : {Pickup}\n" +
                       $"Getter location : {Dst}\n" +
                       $"distance : {Distance}";
            }

        }
    }
}

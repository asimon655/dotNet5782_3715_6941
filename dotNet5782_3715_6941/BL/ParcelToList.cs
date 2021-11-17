using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList : Printable
        {
            public int Id { set; get;  }
            public string SenderName { set; get;  }
            public string TargetName { set; get;  }
            public WeightCategories Weight { set; get;  }
            public Priorities Priorety { set; get;  }
            public ParcelStat ParcelStatus { set; get;  }


        }
    }
}

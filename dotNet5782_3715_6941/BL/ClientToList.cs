using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ClientToList : Printable
        {
            public int Id { set; get;  }
            public String Name { set; get;  }
            public string Phone { set; get;  }
            public int ParcelDeliveredAndGot { set; get;  }
            public int ParcelDeliveredAndNotGot { set; get; }
            public int ParcelGot { set; get;  }
            public int InTheWay { set; get;  }

                 
        }
    } 
}

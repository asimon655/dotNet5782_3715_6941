using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Costumer : OverrideToString
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone_Num { get; set; }
            public IBL.BO.Location Loct { get; set; }
    


        }

    }
}

using System.Collections.Generic;
using BO;


namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {

        public IEnumerable<ClientToList> CostumersPrint()
        {
            List<ClientToList> tmpy = new List<ClientToList>();
            foreach(DO.Costumer x in data.CostumersPrint())
            {
                tmpy.Add(CltToLstC(x));
            }
            return tmpy;
        }
        

    }
} 
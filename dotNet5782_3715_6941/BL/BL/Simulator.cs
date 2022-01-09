using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public void StartSimulator(int droneId, Action refresh, Func<bool> stop)
        {
            new Simulator.Simulator(Instance, droneId, refresh, stop);
        }
    }
}

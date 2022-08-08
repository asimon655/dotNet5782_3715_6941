using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ShowWindow
{
    public delegate void notifyDroneListAdd(BO.Drone drone);
    public delegate void notifyDroneListRefresh(BO.Drone drone);
    public delegate void notifyDroneListDelete(BO.Drone drone);
    public delegate void notifyStataionListAdd();
    public delegate void notifyStataionListDelete();
    public delegate void notifyCostumerListAdd();
    public delegate void notifyCostumerListDelete();
    public delegate void notifyParcelListAdd();
    public delegate void notifyParcelListDelete();
}

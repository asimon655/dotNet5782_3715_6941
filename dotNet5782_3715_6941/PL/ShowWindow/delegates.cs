using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.ShowWindow
{
    public delegate void notifyDroneListAdd(BO.Drone _);
    public delegate void notifyDroneListRefresh(BO.Drone _);
    public delegate void notifyDroneListDelete(BO.Drone _);
    public delegate void notifyStataionListAdd(BO.Station _);
    public delegate void notifyStationListRefresh(BO.Station _);
    public delegate void notifyStataionListDelete(BO.Station _);
    public delegate void notifyCostumerListAdd(BO.Customer _ );
    public delegate void notifyCostumerListRefresh(BO.Customer _ );
    public delegate void notifyCostumerListDelete(BO.Customer _ );
    public delegate void notifyParcelListAdd(BO.Parcel _);
    public delegate void notifyParcelListRefresh(BO.Parcel _);
    public delegate void notifyParcelListDelete(BO.Parcel _);
}

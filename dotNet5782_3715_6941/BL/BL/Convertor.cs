using BO;
using System.Linq;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        #region Convert
        public Drone Convert(DroneList drone)
        {
            Drone newDrone = new Drone()
            {
                Id = drone.Id,
                Weight = drone.Weight,
                Model = drone.Model,
                BatteryStat = drone.Battery,
                Current = drone.Loct,
                DroneStat = drone.DroneStat,
            };
            if (newDrone.DroneStat == DroneStatuses.Delivery && !(drone.ParcelId is null))
            {
                try
                {
                    DO.Parcel parcely = data.GetParcel((int)drone.ParcelId);
                    DO.Customer Sender = data.GetCustomer(parcely.SenderId);
                    DO.Customer Getter = data.GetCustomer(parcely.TargetId);
                    Location SenderLCT = new Location(Sender.Longitude, Sender.Lattitude);
                    Location GetterLCT = new Location(Getter.Longitude, Getter.Lattitude);
                    ParcelInDrone parcelTransfer = new ParcelInDrone()
                    {
                        Id = parcely.Id,
                        Pickup = SenderLCT,
                        Dst = GetterLCT,
                        Distance = ParcelStatusC(parcely) == ParcelStatus.Binded ? calculateDistance(drone.Loct, SenderLCT) :
                                   ParcelStatusC(parcely) == ParcelStatus.PickedUp ? calculateDistance(drone.Loct, GetterLCT) : 0,
                        Weight = (WeightCategories)parcely.Weight,
                        Priorety = (Priorities)parcely.Priority,
                        Sender = new CustomerInParcel() { id = parcely.SenderId, name = Sender.Name },
                        Target = new CustomerInParcel() { id = parcely.TargetId, name = Getter.Name }
                    };
                    newDrone.ParcelTransfer = parcelTransfer;
                }
                catch (DO.IdDosntExists err)
                {
                    throw new IdDosntExists(err);
                }
            }
            return newDrone;
        }
        private Station Convert(DO.Station station)
        {
            return new Station()
            {
                Id = station.Id,
                NumOfFreeOnes = station.ChargeSlots,
                LoctConstant = new Location(station.Longitude, station.Lattitude),
                Name = station.Name,
                DroneInChargeList = (from drones in data.GetDronesCharges(x => x.StaionId == station.Id)
                                     select
    (new DroneCharge()
    {
        DroneId = drones.DroneId,
        Battery = GetDroneToList(drones.DroneId).Battery
    })).ToList()
            };
        }

        private Customer Convert(DO.Customer costumer)
        {
      
            return new Customer()
            {
                Id = costumer.Id,
                Loct = new Location(costumer.Longitude, costumer.Lattitude),
                Name = costumer.Name,
                Phone_Num = costumer.Phone,
                FromClient = (from package in data.GetParcels(x => x.SenderId == costumer.Id) select (CustomerToParcelC(package, new CustomerInParcel() { id = package.TargetId, name =data.GetCustomer(package.TargetId).Name }))).ToList(),
                ToClient = (from package in data.GetParcels(x => x.TargetId == costumer.Id) select (CustomerToParcelC(package, new CustomerInParcel() { id = package.SenderId, name = data.GetCustomer(package.SenderId).Name }))).ToList()
            };
        }

        private Parcel Convert(DO.Parcel parcel)
        {
            try
            {
                DO.Customer sender = data.GetCustomer(parcel.SenderId);
                DO.Customer getter = data.GetCustomer(parcel.TargetId);
                DroneInParcel? droneInParcel = null;
                if (parcel.DroneId is not null)
                {
                    DroneList drone = GetDroneToList((int)parcel.DroneId);
                    droneInParcel = new DroneInParcel() { Id = drone.Id, Battery = drone.Battery, Loct = drone.Loct };
                }
                return new Parcel()
                {
                    Id = parcel.Id,
                    ParcelDelivered = parcel.Delivered,
                    ParcelPickedUp = parcel.PickedUp,
                    ParcelCreation = parcel.Requested,
                    ParcelBinded = parcel.Schedulded,
                    Priority = (Priorities)parcel.Priority,
                    Weight = (WeightCategories)parcel.Weight,
                    ParcelDrone = droneInParcel,
                    SenderParcelToCostumer = new CustomerInParcel() { id = sender.Id, name = sender.Name },
                    GetterParcelToCostumer = new CustomerInParcel() { id = getter.Id, name = getter.Name }
                };
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        #endregion

        #region ConvertList
        private CustomerList ConvertList(DO.Customer gety)
        {
            return new CustomerList()
            {
                Id = gety.Id
,
                Name = gety.Name
,
                Phone = gety.Phone
,
                ParcelDeliveredAndGot = data.CountParcels(x => x.SenderId == gety.Id && ParcelStatusC(x) == ParcelStatus.Delivered)
,
                InTheWay = data.CountParcels(x => x.SenderId == gety.Id && ParcelStatusC(x) != ParcelStatus.Delivered)
,
                ParcelGot = data.CountParcels(x => x.TargetId == gety.Id && ParcelStatusC(x) == ParcelStatus.Delivered)
,
                ParcelDeliveredAndNotGot = data.CountParcels(x => x.SenderId == gety.Id && ParcelStatusC(x) != ParcelStatus.Delivered)
            };
        }

        private ParcelList ConvertList(DO.Parcel x)
        {
            return new ParcelList()
            {
                Id = x.Id,
                ParcelStatus = ParcelStatusC(x),
                Priorety = (Priorities)x.Priority,
                SenderName = data.GetCustomer(x.SenderId).Name,
                TargetName = data.GetCustomer(x.TargetId).Name,
                Weight = (WeightCategories)x.Weight
            };
        }

        private StationList ConvertList(DO.Station station)
        {
            return new StationList()
            {
                Id = station.Id,
                Name = station.Name,
                FreePorts = station.ChargeSlots,
                BusyPorts = data.CountDronesCharges(x => x.StaionId == station.Id)
            };
        }
        #endregion

        #region other Converts
        private ParcelStatus ParcelStatusC(DO.Parcel parcel)
        {

            int caseNum = -1;
            if (!(parcel.Schedulded is null))
            {
                caseNum++;
                if (!(parcel.Requested is null))
                {
                    caseNum++;
                    if (!(parcel.PickedUp is null))
                    {
                        caseNum++;
                        if (!(parcel.Delivered is null))
                        {
                            caseNum++;
                        }
                    }
                }
            }
            if (caseNum == -1)
            {
                throw new EnumOutOfRange("the parcel is not even decleared ", -1);
            }

            return (ParcelStatus)caseNum;
        }
        public static ParcelStatus ParcelStatusC(Parcel parcel)
        {

            int caseNum = -1;
            if (!(parcel.ParcelCreation is null))
            {
                caseNum++;
                if (!(parcel.ParcelBinded is null))
                {
                    caseNum++;
                    if (!(parcel.ParcelPickedUp is null))
                    {
                        caseNum++;
                        if (!(parcel.ParcelDelivered is null))
                        {
                            caseNum++;
                        }
                    }
                }
            }
            if (caseNum == -1)
            {
                throw new EnumOutOfRange("the parcel is not even decleared ", -1);
            }

            return (ParcelStatus)caseNum;
        }
        private ParcelInCustomer CustomerToParcelC(DO.Parcel parcel, CustomerInParcel parentCustomer)
        {
            return new ParcelInCustomer()
            {
                Id = parcel.Id,
                Priority = (Priorities)parcel.Priority,
                Status = ParcelStatusC(parcel),
                Weight = (WeightCategories)parcel.Weight,
                ParentCustomer = parentCustomer
            };
        }
        #endregion
    }
}
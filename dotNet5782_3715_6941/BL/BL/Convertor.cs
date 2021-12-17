using BO;
using System.Linq;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
         private CustomerList CltToLstC(DO.Customer gety) =>  new CustomerList() { 
                Id = gety.Id 
                ,Name=gety.Name 
                ,Phone=gety.Phone 
                ,ParcelDeliveredAndGot =data.CountParcels(x => x.SenderId == gety.Id && ParcelStatusC(x) == ParcelStatus.Delivered)  
                ,InTheWay= data.CountParcels(x => x.SenderId == gety.Id && ParcelStatusC(x) != ParcelStatus.Delivered)
                ,ParcelGot = data.CountParcels(x => x.TargetId == gety.Id && ParcelStatusC(x) == ParcelStatus.Delivered) 
                ,ParcelDeliveredAndNotGot = data.CountParcels(x => x.SenderId == gety.Id && ParcelStatusC(x) != ParcelStatus.Delivered) };

        private ParcelStatus ParcelStatusC(DO.Parcel parcel)
        {

            int caseNum = 4;
            if (!(parcel.PickedUp is null))
                caseNum--; 
            if (!(parcel.Delivered is null))
                caseNum--;
            if (!(parcel.Schedulded is null))
                caseNum--;
            if (!(parcel.Requested is null))
                caseNum--;
            if (caseNum == 4)
                throw new EnumOutOfRange("4 for parcelstat is forbidden ",4); 
            return (ParcelStatus)caseNum; 
        }
        
        private ParcelInCustomer CustomerToParcelC(DO.Parcel parcel, CustomerInParcel parentCustomer)
        {
            return new ParcelInCustomer() {
                Id = parcel.Id,
                Priority = (Priorities)parcel.Priority,
                Status = ParcelStatusC(parcel),
                Weight = (WeightCategories)parcel.Weight,
                ParentCustomer = parentCustomer
            };
        }
        private Drone DronesC(DroneList drone)
        {
            Drone newDrone = new Drone(){
                Id = drone.Id,
                Weight = (WeightCategories)drone.Weight,
                Model = drone.Model,
                BatteryStat = drone.Battery,
                Current = drone.Loct,
                DroneStat = drone.DroneStat,
            };
            if (newDrone.DroneStat == DroneStatuses.Delivery)
            {
                int parcelyId = getBindedUndeliveredParcel(newDrone.Id);
                try
                {
                    DO.Parcel parcely = data.GetParcel(parcelyId);
                    DO.Customer Sender = data.GetCustomer(parcely.SenderId);
                    DO.Customer Getter = data.GetCustomer(parcely.TargetId);  
                    Location SenderLCT = new Location(Sender.Longitude, Sender.Lattitude);
                    Location GetterLCT = new Location(Getter.Longitude, Getter.Lattitude);
                    ParcelInDrone parcelTransfer = new ParcelInDrone() {
                    Id = parcely.Id , 
                    Pickup = SenderLCT   , 
                    Dst =   GetterLCT,
                    Distance =calculateDistance(SenderLCT , GetterLCT ) , 
                    Weight = (WeightCategories)parcely.Weight , 
                    Priorety = (Priorities)parcely.Priority , 
                    Sender = new CustomerInParcel() { id = parcely.SenderId, name = data.GetCustomer(parcely.SenderId).Name } , 
                    Target = new CustomerInParcel() { id = parcely.TargetId, name = data.GetCustomer(parcely.TargetId).Name }
                };
                newDrone.ParcelTransfer = parcelTransfer;
                }
                catch (DO.IdDosntExists  err) {
                    throw new IdDosntExists(err); 


                } 
              
            }
            return newDrone;
        }

        private Station StationC(DO.Station station) => new Station(){
                Id = station.Id,
                NumOfFreeOnes = station.ChargeSlots,
                LoctConstant = new Location(station.Longitude, station.Lattitude),
                Name = station.Name , 
                DroneInChargeList = (from drones in data.GetDronesCharges(x => x.StaionId == station.Id) select (new DroneCharge() { 
                    DroneId = drones.DroneId ,
                    Battery = GetDroneToList(drones.DroneId).Battery }  ) ).ToList ()  
                };


        private Customer CostumerC(DO.Customer costumer) => new Customer() { 
            Id = costumer.Id, 
            Loct = new Location(costumer.Longitude, costumer.Lattitude), 
            Name = costumer.Name, 
            Phone_Num = costumer.Phone , 
            FromClient = ( from package in data.GetParcels(x => x.SenderId == costumer.Id)  select (CustomerToParcelC(package, new CustomerInParcel() {id = costumer.Id, name = costumer.Name})) ).ToList() ,
            ToClient = (from package in data.GetParcels(x => x.TargetId == costumer.Id) select (CustomerToParcelC(package, new CustomerInParcel() {id = costumer.Id, name = costumer.Name}))).ToList()
        };


        private Parcel ParcelC(DO.Parcel parcel)
        {
            try
            {
                DO.Customer sender = data.GetCustomer(parcel.SenderId);
                DO.Customer getter = data.GetCustomer(parcel.TargetId);
                DroneInParcel? droneInParcel = null;
                if (ParcelStatusC(parcel) != ParcelStatus.Declared)
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

    }
} 
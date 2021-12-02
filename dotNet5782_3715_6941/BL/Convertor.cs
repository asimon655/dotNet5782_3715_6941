﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class Bl : IBL.Ibl
    {
         private ClientToList CltToLstC(IDAL.DO.Costumer gety) =>  new ClientToList() { 
                Id = gety.Id 
                ,Name=gety.Name 
                ,Phone=gety.Phone 
                ,ParcelDeliveredAndGot =data.CountParcels(x => x.SenderId == gety.Id && ParcelStatC(x) == ParcelStat.Delivered)  
                ,InTheWay= data.CountParcels(x => x.SenderId == gety.Id && ParcelStatC(x) != ParcelStat.Delivered)
                ,ParcelGot = data.CountParcels(x => x.TargetId == gety.Id && ParcelStatC(x) == ParcelStat.Delivered) 
                ,ParcelDeliveredAndNotGot = data.CountParcels(x => x.SenderId == gety.Id && ParcelStatC(x) != ParcelStat.Delivered) };

        private ParcelStat ParcelStatC(IDAL.DO.Parcel parcel)
        {
            int caseNum = 4;
            if (!(parcel.PickedUp is null))
                caseNum--; 
            else if (!(parcel.Delivered is null))
                caseNum--;
            else if (!(parcel.Schedulded is null))
                caseNum--;
            else if (!(parcel.Requested is null))
                caseNum--;
            if (caseNum == 4)
                throw new EnumOutOfRange("4 for parcelstat is forbidden ",4); 
            return (ParcelStat)caseNum; 
        }
        
        private CustomerToParcel CustomerToParcelC(IDAL.DO.Parcel parcel, ParcelToCostumer parentCustomer)
        {
            return new CustomerToParcel() {
                Id = parcel.Id,
                Priority = (Priorities)parcel.Priority,
                Status = ParcelStatC(parcel),
                Weight = (WeightCategories)parcel.Weight,
                ParentCustomer = parentCustomer
            };
        }
        private Drone DronesC(DroneToList drone)
        {
            Drone newDrone = new Drone(){
                Id = drone.Id,
                Weight = (WeightCategories)drone.Weight,
                Model = drone.Model,
                BatteryStat = drone.BatteryStat,
                Current = drone.Current,
                DroneStat = drone.DroneStat,
            };
            if (newDrone.DroneStat == DroneStatuses.Delivery)
            {
                int parcelyId = getBindedUndeliveredParcel(newDrone.Id);
                try
                {
                    IDAL.DO.Parcel parcely = data.PullDataParcel(parcelyId);
                    IDAL.DO.Costumer Sender = data.PullDataCostumer(parcely.SenderId);
                    IDAL.DO.Costumer Getter = data.PullDataCostumer(parcely.TargetId);  
                    Location SenderLCT = new Location(Sender.Longitude, Sender.Lattitude);
                    Location GetterLCT = new Location(Getter.Longitude, Getter.Lattitude);
                    ParcelInTransfer parcelTransfer = new ParcelInTransfer() {
                    Id = parcely.Id , 
                    Pickup = SenderLCT   , 
                    Dst =   GetterLCT,
                    Distance =calculateDistance(SenderLCT , GetterLCT ) , 
                    Weight = (WeightCategories)parcely.Weight , 
                    Priorety = (Priorities)parcely.Priority , 
                    Sender = new ParcelToCostumer() { id = parcely.SenderId, name = data.PullDataCostumer(parcely.SenderId).Name } , 
                    Target = new ParcelToCostumer() { id = parcely.TargetId, name = data.PullDataCostumer(parcely.TargetId).Name }
                };
                newDrone.ParcelTransfer = parcelTransfer;
                }
                catch (IDAL.DO.IdDosntExists  err) {
                    throw new IdDosntExists(err); 


                } 
              
            }
            return newDrone;
        }

        private BaseStation StationC(IDAL.DO.Station station) => new BaseStation(){
                Id = station.Id,
                NumOfFreeOnes = station.ChargeSlots,
                LoctConstant = new Location(station.Longitude, station.Lattitude),
                Name = station.Name , 
                DroneInChargeList = (from drones in data.GetDronesCharges(x => x.StaionId == station.Id) select (new DroneInCharge() { 
                    id = drones.DroneId ,
                    BatteryStat = GetDroneToList(drones.DroneId).BatteryStat }  ) ).ToList ()  
                };


        private Costumer CostumerC(IDAL.DO.Costumer costumer) => new Costumer() { 
            Id = costumer.Id, 
            Loct = new Location(costumer.Longitude, costumer.Lattitude), 
            Name = costumer.Name, 
            Phone_Num = costumer.Phone , 
            FromClient = ( from package in data.GetParcels(x => x.SenderId == costumer.Id)  select (CustomerToParcelC(package, new ParcelToCostumer() {id = costumer.Id, name = costumer.Name})) ).ToList() ,
            ToClient = (from package in data.GetParcels(x => x.TargetId == costumer.Id) select (CustomerToParcelC(package, new ParcelToCostumer() {id = costumer.Id, name = costumer.Name}))).ToList()
        };


        private Parcel ParcelC(IDAL.DO.Parcel parcel)
        {
            try
            {
                IDAL.DO.Costumer sender = data.PullDataCostumer(parcel.SenderId);
                IDAL.DO.Costumer getter = data.PullDataCostumer(parcel.TargetId);
                ParcelToDrone? droneInParcel = null;
                if (ParcelStatC(parcel) != ParcelStat.Declared)
                {
                    DroneToList drone = GetDroneToList((int)parcel.DroneId);
                    droneInParcel = new ParcelToDrone() { Id = drone.Id, BatteryStat = drone.BatteryStat, Loct = drone.Current };
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
                    SenderParcelToCostumer = new ParcelToCostumer() { id = sender.Id, name = sender.Name },
                    GetterParcelToCostumer = new ParcelToCostumer() { id = getter.Id, name = getter.Name }
                };
            }
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            } 
        }

    }
} 
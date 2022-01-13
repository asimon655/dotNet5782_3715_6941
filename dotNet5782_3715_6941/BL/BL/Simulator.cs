using BO;
using System;
using System.Runtime.CompilerServices;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public void StartSimulator(int droneId, Action refresh, Func<bool> stop)
        {
            new Simulator.Simulator(Instance, droneId, refresh, stop);
        }
        public void SimulatorUpdateBattary(int droneId, double battary)
        {
            GetDroneToList(droneId).Battery = battary;
        }
        public void SimulatorUpdateLocation(int droneId, BO.Location location)
        {
            GetDroneToList(droneId).Loct = location;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int SimulatorDroneSaveChargeSlot(int droneId)
        {
            lock (data) lock (instance)
                {
                    DroneList drony = GetDroneToList(droneId);
                    if (drony.DroneStat != DroneStatuses.Free)
                    {
                        throw new EnumNotInRightStatus<DroneStatuses>("drone is not not Free  , it is :  ", drony.DroneStat);
                    }

                    try
                    {
                        int stationID = getClosesStation(drony.Loct);
                        DO.Station station = data.GetStation(stationID);
                        Location stationLoct = new Location(station.Longitude, station.Lattitude);
                        double powerUsage = getPowerUsage(drony.Loct, stationLoct);
                        if (drony.Battery < powerUsage)
                        {
                            throw new CantReachToDest("the charging port os too far to go with the current battery precantage", drony.Battery, powerUsage);
                        }

                        station.ChargeSlots -= 1;

                        DO.DroneCharge chargingport = new DO.DroneCharge() { DroneId = drony.Id, StaionId = station.Id };

                        data.AddDroneCharge(chargingport);

                        data.UpdateStations(station);

                        return stationID;
                    }
                    catch (DO.IdAlreadyExists err)
                    {
                        throw new IdAlreadyExists(err);
                    }
                    catch (DO.IdDosntExists err)
                    {
                        throw new IdDosntExists(err);
                    }
                }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SimulatorDroneReleaseChargeSlot(int droneId)
        {
            lock (data)
            {
                try
                {
                    DO.DroneCharge charge = data.GetDroneCharge(droneId);

                    DO.Station station = data.GetStation(charge.StaionId);
                    station.ChargeSlots -= 1;

                    data.UpdateStations(station);

                    data.DeleteDroneCharge(droneId);
                }
                catch (DO.IdAlreadyExists err)
                {
                    throw new IdAlreadyExists(err);
                }
                catch (DO.IdDosntExists err)
                {
                    throw new IdDosntExists(err);
                }
            }
        }
    }
}

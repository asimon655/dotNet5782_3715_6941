using BO;
using System.Collections.Generic;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
       
        public void UpdateStation(int stationId, int? stationName = null, int? stationChargeSlots = null)
        {
            try
            {
                DO.Station Stationy = data.GetStation(stationId);
                if (!(stationName is null))
                {
                    Stationy.Name = (int)stationName;
                }
                if (!(stationChargeSlots is null))
                {
                    int chargeSlots = (int)(stationChargeSlots - data.CountDronesCharges(x => x.StaionId == stationId));
                    if (chargeSlots < 0)
                    {
                        throw new InValidSumOfChargeSlots("you currently using more staions the you want to update :: ");
                    }
                    Stationy.ChargeSlots = chargeSlots;
                }
                data.UpdateStations(Stationy);
            }
            catch (DO.IdDosntExists err) {
                throw new IdDosntExists(err);
            } 
        }
        public Station GetStation(int stationId)
        {

            try
            {
                Station TmpStation = StationC(data.GetStation(stationId));
                List<DroneCharge> dronesInCharges = new List<DroneCharge>();
                foreach (var droneCharge in data.GetDronesCharges(x => x.StaionId == stationId))
                {
                    DroneList drone = GetDroneToList(droneCharge.DroneId);
                    // check if the drone exsists
                    DroneCharge droneInCharge = new DroneCharge
                    {
                        DroneId = drone.Id,
                        Battery = drone.Battery
                    };

                    dronesInCharges.Add(droneInCharge);
                }

                return TmpStation;
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            }

           
        }

        public void AddStation(Station station)
        {
            if (station.LoctConstant.Lattitude > 90 || station.LoctConstant.Lattitude < -90 || station.LoctConstant.Longitude > 180 || station.LoctConstant.Longitude < -180)
                throw new LocationOutOfRange("the Location Values are out of boundries  :  ", station.LoctConstant.Longitude, station.LoctConstant.Lattitude);
            
            DO.Station StationTmp = new DO.Station() {Id = station.Id,ChargeSlots=station.NumOfFreeOnes,Lattitude=station.LoctConstant.Lattitude,Longitude=station.LoctConstant.Longitude
            ,Name=station.Name};
            try
            {
                data.AddStation(StationTmp);
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err);
            } 
        }


       
       


    }
}

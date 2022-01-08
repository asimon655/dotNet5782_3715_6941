using System.Collections.Generic;
using System.Linq;
using BO;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public Station GetStation(int stationId)
        {
            try
            {
                return Convert(data.GetStation(stationId));
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

            DO.Station StationTmp = new DO.Station()
            {
                Id = station.Id,
                ChargeSlots = station.NumOfFreeOnes,
                Lattitude = station.LoctConstant.Lattitude,
                Longitude = station.LoctConstant.Longitude,
                Name = station.Name
            };
            try
            {
                data.AddStation(StationTmp);
            }
            catch (DO.IdAlreadyExists err)
            {
                throw new IdAlreadyExists(err);
            } 
        }
        public void UpdateStation(int stationId, string? stationName = null, int? stationChargeSlots = null)
        {
            try
            {
                DO.Station Stationy = data.GetStation(stationId);
                if (stationName is not null)
                {
                    Stationy.Name = stationName;
                }
                if (stationChargeSlots is not null)
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
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        public void DeleteStation(int id)
        {
            try
            {
                data.DeleteStation(id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }
        public IEnumerable<StationList> GetStations()
        {
            return data.GetStations().Select(ConvertList);
        }
        public IEnumerable<StationList> GetStationsWithFreePorts()
        {
            return data.GetStations(x => x.ChargeSlots > 0).Select(ConvertList);
        }
        public IEnumerable<StationList> GetStationsFiltered(IEnumerable<int>? FreePorts, IEnumerable<int>? BusyPorts)
        {
            return data.GetStations(x => (FreePorts is null || FreePorts.Contains(x.ChargeSlots)) &&
                                        (BusyPorts is null || BusyPorts.Contains(data.CountDronesCharges(s => s.StaionId == x.Id))))
                .Select(ConvertList);

        }
    }
}

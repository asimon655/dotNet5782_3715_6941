using BO;
using System.Collections.Generic;


namespace BO
{
    public class BaseStaionToList 
    {
        public int Id { set; get; }
        public int Name { set; get; }
        public int NumOfNotFreeOne { set; get;  } 
        public int NumOfFreeOnes { set; get; }

        public override string ToString()
        {
            return $"Id : {Id}\n" +
                    $"Name : {Name}\n" +
                    $"occupied charging slots : {NumOfNotFreeOne}\n" +
                    $"free charging slots : {NumOfFreeOnes}";
        }
    }
}

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public IEnumerable<BaseStaionToList> BaseStaionsFreePortsPrint()
        {
            List<BaseStaionToList> tmp = new List<BaseStaionToList>();
            // loop all stations with free charging slots
            foreach (var station in data.GetStations(x => x.ChargeSlots > 0))
            {
                int numOfNotFr = data.CountDronesCharges(x => x.StaionId == station.Id);
                tmp.Add(new BaseStaionToList() { Id = station.Id, Name = station.Name, NumOfFreeOnes = station.ChargeSlots, NumOfNotFreeOne = numOfNotFr });
            }
            return tmp;

        }

        public IEnumerable<BaseStaionToList> StaionsPrint()
        {
            List<BaseStaionToList> tmp= new List<BaseStaionToList>();
            foreach (var station in data.StationsPrint())
            {
                int numOfNotFr = data.CountDronesCharges(x=>x.StaionId ==  station.Id);
                tmp.Add(new BaseStaionToList() { Id = station.Id, Name = station.Name, NumOfFreeOnes = station.ChargeSlots, NumOfNotFreeOne = numOfNotFr  });
            }
            return tmp; 
        }
       

    }
}
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    {
        public class BaseStaionToList : Printable
        {
            public int Id { set; get; }
            public int Name { set; get; }
            public int NumOfNotFreeOne { set; get;  } 
            public int NumOfFreeOnes { set; get; }
            
        }
    }
}

namespace BL
{
    public partial class Bl : IBL.Ibl
    {
        public IEnumerable<BaseStaionToList> BaseStaionsFreePortsPrint()
        {
            List<BaseStaionToList> tmp = new List<BaseStaionToList>();
            foreach (var station in data.StaionsFreePortsPrint())
            {
                int numOfNotFr = data.DronesChargesPrint().Count(x => x.StaionId == station.Id);
                tmp.Add(new BaseStaionToList() { Id = station.Id, Name = station.Name, NumOfFreeOnes = station.ChargeSlots, NumOfNotFreeOne = numOfNotFr });
            }
            return tmp;

        }

        public IEnumerable<BaseStaionToList> StaionsPrint()
        {
            List<BaseStaionToList> tmp= new List<BaseStaionToList>();
            foreach (var station in data.StationsPrint())
            {
                int numOfNotFr = data.DronesChargesPrint().Count(x=>x.StaionId==  station.Id);
                tmp.Add(new BaseStaionToList() { Id = station.Id, Name = station.Name, NumOfFreeOnes = station.ChargeSlots, NumOfNotFreeOne = numOfNotFr  });
            }
            return tmp; 
        }
       

    }
}
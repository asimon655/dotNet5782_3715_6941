using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    {
        public class Costumer : Printable
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone_Num { get; set; }
            public IBL.BO.Location Loct { get; set; }
            public List<Parcel> FromClient { get; init; } = new List<Parcel> ();
            public List<Parcel> ToClient { get; init; } = new List<Parcel> () ; 

        }

    }
}
namespace BL
{
    public partial class Bl : IBL.Ibl
    {
        public void AddCostumer(Costumer costumer)
        {
            IDAL.DO.Costumer CostumerTmp = new IDAL.DO.Costumer() { 
                Id = costumer.Id, 
                Lattitude = costumer.Loct.Lattitude, 
                Longitude = costumer.Loct.Longitude, 
                Name = costumer.Name, 
                Phone = costumer.Phone_Num,
                
            };
            data.AddCostumer(CostumerTmp);
        }
        public Costumer PullDataCostumer(int id)
        {
            return CostumerC(data.PullDataCostumer(id));
        }
        public void UpdateCostumer(int costumerId, string costumerName = null, string costumerPhone = null)
        {
            IDAL.DO.Costumer Costumery = new IDAL.DO.Costumer() { Id = costumerId,
                Name=costumerName  ,
                Phone=costumerPhone };
            data.UpdateCostumers(Costumery);
        }



    }
}

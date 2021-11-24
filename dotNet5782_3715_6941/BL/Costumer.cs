using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    {
        public class Costumer 
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone_Num { get; set; }
            public IBL.BO.Location Loct { get; set; }
            public List<CustomerToParcel> FromClient { get; set; } 
            public List<CustomerToParcel> ToClient { get; set; }

            public override string ToString()
            {
                return $"Id : {Id}\n" +
                       $"Name : {Name}\n" +
                       $"location : {Loct}\n" +
                       $"phone : {Phone_Num}\n" +
                       $"parceles sent to him : {string.Join('\n', ToClient)}\n" +
                       $"parceles he sent : {string.Join('\n', FromClient)}";
            }
        }

    }
}
namespace BL
{
    public partial class Bl : IBL.Ibl
    {
        public void AddCostumer(Costumer costumer)
        {
            if (costumer.Loct.Lattitude > 90 || costumer.Loct.Lattitude < -90 || costumer.Loct.Longitude > 180 || costumer.Loct.Longitude < -180)
                throw new LocationOutOfRange("the Location Values are out of boundries  :  ", costumer.Loct.Longitude, costumer.Loct.Lattitude);
            IDAL.DO.Costumer CostumerTmp = new IDAL.DO.Costumer() { 
                Id = costumer.Id, 
                Lattitude = costumer.Loct.Lattitude, 
                Longitude = costumer.Loct.Longitude, 
                Name = costumer.Name, 
                Phone = costumer.Phone_Num,
                
            };
            try
            {
                data.AddCostumer(CostumerTmp);
            }
            catch (IDAL.DO.IdAlreadyExists err)
            {


                throw new IdAlreadyExists( err); 

            
            } 
        }
        public Costumer PullDataCostumer(int id)
        {
            try
            {
                return CostumerC(data.PullDataCostumer(id));
            }
            catch (IDAL.DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            
            }
          
        }
        public void UpdateCostumer(int costumerId, string costumerName = null, string costumerPhone = null)
        {

            
            try
            {

                IDAL.DO.Costumer Costumery = data.PullDataCostumer(costumerId);
                if (!(costumerName is null))
                    Costumery.Name = costumerName;
                if (!(costumerPhone is null))
                    Costumery.Phone = costumerPhone;
                data.UpdateCostumers(Costumery);
     
                    
            }
            catch (IDAL.DO.IdDosntExists err) {

                throw new IdDosntExists(err); 
            
            
            } 
        }



    }
}

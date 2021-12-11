using BO;
using System.Collections.Generic;


namespace BO
{
    public class Costumer 
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone_Num { get; set; }
        public Location Loct { get; set; }
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

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public void AddCostumer(Costumer costumer)
        {
            if (costumer.Loct.Lattitude > 90 || costumer.Loct.Lattitude < -90 || costumer.Loct.Longitude > 180 || costumer.Loct.Longitude < -180)
                throw new LocationOutOfRange("the Location Values are out of boundries  :  ", costumer.Loct.Longitude, costumer.Loct.Lattitude);
            DO.Costumer CostumerTmp = new DO.Costumer() { 
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
            catch (DO.IdAlreadyExists err)
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
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err); 
            
            }
          
        }
        public void UpdateCostumer(int costumerId, string costumerName = null, string costumerPhone = null)
        {

            
            try
            {

                DO.Costumer Costumery = data.PullDataCostumer(costumerId);
                if (!(costumerName is null))
                    Costumery.Name = costumerName;
                if (!(costumerPhone is null))
                    Costumery.Phone = costumerPhone;
                data.UpdateCostumers(Costumery);
     
                    
            }
            catch (DO.IdDosntExists err) {

                throw new IdDosntExists(err); 
            
            
            } 
        }



    }
}

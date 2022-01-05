using BO;

namespace BL
{
    public sealed partial class Bl : BlApi.Ibl
    {
        public void AddCustomer(Customer costumer)
        {
            if (costumer.Loct.Lattitude > 90 || costumer.Loct.Lattitude < -90 || costumer.Loct.Longitude > 180 || costumer.Loct.Longitude < -180)
                throw new LocationOutOfRange("the Location Values are out of boundries  :  ", costumer.Loct.Longitude, costumer.Loct.Lattitude);
            DO.Customer CostumerTmp = new DO.Customer() { 
                Id = costumer.Id, 
                Lattitude = costumer.Loct.Lattitude, 
                Longitude = costumer.Loct.Longitude, 
                Name = costumer.Name, 
                Phone = costumer.Phone_Num,
                
            };
            try
            {
                data.AddCustomer(CostumerTmp);
            }
            catch (DO.IdAlreadyExists err)
            {


                throw new IdAlreadyExists( err); 

            
            } 
        }
        public Customer GetCostumer(int id)
        {
            try
            {
                return CostumerC(data.GetCustomer(id));
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

                DO.Customer Costumery = data.GetCustomer(costumerId);
                if (!(costumerName is null))
                    Costumery.Name = costumerName;
                if (!(costumerPhone is null))
                    Costumery.Phone = costumerPhone;
                data.UpdateCustomer(Costumery);
     
                    
            }
            catch (DO.IdDosntExists err) 
            {
                throw new IdDosntExists(err);
            } 
        }

        public void DeleteCustomer(int id)
        {
            try
            {
                data.DeleteCustomer(id);
            }
            catch (DO.IdDosntExists err)
            {
                throw new IdDosntExists(err);
            }
        }

    }
}

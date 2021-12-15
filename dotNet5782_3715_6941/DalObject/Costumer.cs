using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        public void AddCustomer(Customer costumer)
        {
            // if we find that the id is already taken by another costumer
            if (DataSource.Costumers.Any(s => s.Id == costumer.Id))
            {
                throw new IdAlreadyExists("the Id Costumer is already taken", costumer.Id);
            }

            DataSource.Costumers.Add(costumer);
        }
        public Customer GetCustomer(int id)
        {
            Customer costumer = DataSource.Costumers.Find(s => s.Id == id);
            /// if the Costumer wasnt found we throwing an error
            if (costumer.Id != id)
            {
                throw new IdDosntExists("the Id couldnt be found", id);
            }
            return costumer;
        }
        public void UpdateCustomer(Customer costumer)
        {
            // if we cant find any costumer with the id we throw an error
            if (!DataSource.Costumers.Any(s => s.Id == costumer.Id))
            {
                throw new IdDosntExists("the Id couldnt be found ", costumer.Id);
            }

            Update(DataSource.Costumers, costumer);
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return DataSource.Costumers;
        }
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> expr)
        {
            return DataSource.Costumers.FindAll(expr);
        }
    }
}

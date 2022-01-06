using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        public void AddCustomer(Customer customer)
        {
            customer.IsDeleted = false;
            // if we find that the id is already taken by another costumer
            if (DataSource.Costumers.Any(s => s.Id == customer.Id))
            {
                throw new IdAlreadyExists("the Id Costumer is already taken", customer.Id);
            }

            DataSource.Costumers.Add(customer);
        }
        public Customer GetCustomer(int id)
        {
            Customer costumer = DataSource.Costumers.Find(s => !s.IsDeleted && s.Id == id);
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
            if (Update(DataSource.Costumers, costumer) == -1)
            {
                throw new IdDosntExists("the Id couldnt be found ", costumer.Id);
            }
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return DataSource.Costumers.FindAll(s => !s.IsDeleted);
        }
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> expr)
        {
            return DataSource.Costumers.FindAll(s => !s.IsDeleted && expr(s));
        }
        public int CountCustomers(Func<Customer, bool> expr)
        {
            return DataSource.Costumers.Count(s => !s.IsDeleted && expr(s));
        }
        public void DeleteCustomer(int id)
        {
            // if we cant find any costumer with the id we throw an error
            if (Delete(DataSource.Costumers, id) == -1)
            {
                throw new IdDosntExists("the Id couldnt be found ", id);
            }
        }
    }
}

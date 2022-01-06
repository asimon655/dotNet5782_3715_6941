using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DO;


namespace Dal
{
    internal sealed partial class DalXml : DalApi.IDal
    {
        public void AddCustomer(Customer customer)
        {
            customer.IsDeleted = false;

            List<Customer> customers = Read<Customer>();
            
            if (customers.Any(x => x.Id == customer.Id))
                throw new IdAlreadyExists("there is already a customer with that id", customer.Id);

            customers.Add(customer);

            Write(customers);
        }

        public Customer GetCustomer(int id)
        {
            List<Customer> customers = Read<Customer>();

            Customer customer = customers.Find(s => !s.IsDeleted && s.Id == id);

            /// if the Customer wasnt found throw error
            if (customer.Id != id)
            {
                throw new IdDosntExists("the Id could not be found", id);
            }
            return customer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return Read<Customer>().FindAll(s => !s.IsDeleted);
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> expr)
        {
            return Read<Customer>().FindAll(s => !s.IsDeleted && expr(s));
        }
        public int CountCustomers(Func<Customer, bool> expr)
        {
            return Read<Customer>().Count(s => !s.IsDeleted && expr(s));
        }

        public void UpdateCustomer(Customer customer)
        {
            List<Customer> customers = Read<Customer>();

            if (Update(customers, customer) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", customer.Id);
            }

            Write(customers);
        }
        public void DeleteCustomer(int id)
        {
            List<Customer> customers = Read<Customer>();

            if (Delete(customers, id) == -1)
            {
                throw new IdDosntExists("the Id Drone is dosnt exists", id);
            }

            Write(customers);
        }
    }
}

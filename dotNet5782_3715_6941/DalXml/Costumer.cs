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
            List<Customer> customers = Read<Customer>();
            
            if (customers.Any(x => x.Id == customer.Id))
                throw new IdAlreadyExists("there is already a customer with that id", customer.Id);

            customers.Add(customer);

            Write(customers);
        }

        public Customer GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> expr)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer costumer)
        {
            throw new NotImplementedException();
        }
    }
}

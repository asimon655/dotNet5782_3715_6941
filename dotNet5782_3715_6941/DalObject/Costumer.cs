using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        public void AddCostumer(Costumer costumer)
        {
            // if we find that the id is already taken by another costumer
            if (DataSource.Costumers.Any(s => s.Id == costumer.Id))
            {
                throw new IdAlreadyExists("the Id Costumer is already taken", costumer.Id);
            }

            DataSource.Costumers.Add(costumer);
        }
        public Costumer PullDataCostumer(int id)
        {
            Costumer costumer = DataSource.Costumers.Find(s => s.Id == id);
            /// if the Costumer wasnt found we throwing an error
            if (costumer.Id != id)
            {
                throw new IdDosntExists("the Id couldnt be found", id);
            }
            return costumer;
        }
        public IEnumerable<Costumer> CostumersPrint()
        {
            return DataSource.Costumers;
        }
        public void UpdateCostumers(Costumer costumer)
        {
            // if we cant find any costumer with the id we throw an error
            if (!DataSource.Costumers.Any(s => s.Id == costumer.Id))
            {
                throw new IdDosntExists("the Id couldnt be found", costumer.Id);
            }

            Update(DataSource.Costumers, costumer);
        }

        public IEnumerable<Costumer> GetCostumers(Predicate<Costumer> expr)
        {
            return DataSource.Costumers.FindAll(expr);
        }
    }
}

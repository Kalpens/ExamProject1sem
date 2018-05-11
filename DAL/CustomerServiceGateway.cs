using System;
using System.Collections.Generic;
using System.Text;
using BE;

namespace DAL
{
    class CustomerServiceGateway : IServiceGateway<Customer>
    {
        private static CustomerServiceGateway _instance;

        public static CustomerServiceGateway Instance
            => _instance ?? (_instance = new CustomerServiceGateway());

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> Get()
        {
            throw new NotImplementedException();
        }

        public bool Create(Customer newObject)
        {
            throw new NotImplementedException();
        }

        public Customer Update(Customer updateObject)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}

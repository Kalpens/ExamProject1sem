using System;
using System.Collections.Generic;
using System.Text;
using BE;

namespace DAL
{
    public class DALFacade
    {
        public IServiceGateway<Customer> GetCustomerServiceGateway()
        {
            return CustomerServiceGateway.Instance;
        }
    }
}

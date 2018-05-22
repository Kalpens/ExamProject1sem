using System;
using System.Collections.Generic;
using System.Text;
using BE;

namespace DAL
{
    class InjectionTest
    {
        /// <summary>
        /// Bad way of countering sql injection, as making single
        /// quote as double quote can be worked around
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public Customer PrepareCustomer(Customer customer)
        {
            customer.Address = SafeSqlLiteral(customer.Address);
            customer.Name = SafeSqlLiteral(customer.Name);
            return customer;
        }
        private string SafeSqlLiteral(string inputSQL)
        {
            return inputSQL.Replace("'", "''");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using BE;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly IServiceGateway<Customer> _customerGateway = new DALFacade().GetCustomerServiceGateway();

        private readonly DbConnection _connection;

        public CustomerController(DbConnection connection)
        {
            this._connection = connection;
        }
        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {

            return _customerGateway.Get();
        }

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Customer> Get(int id)
        {
            using (var lookupCommand = _connection.CreateCommand())
            {
                lookupCommand.CommandText = @"
                    SELECT * FROM customers Where CustomerId = " + id;
                var reader = await lookupCommand.ExecuteReaderAsync();
                Customer customer = new Customer()
                {
                };
                while (await reader.ReadAsync())

                {
                    customer.Id = reader.GetInt32(0);
                    customer.BirthDate = reader.GetDateTime(2);
                    customer.Name = reader.GetString(1);
                    customer.PhoneNumber = reader.GetInt32(4);
                    customer.Address = reader.GetString(3);
                }
                return customer;
            }
            //return _customerGateway.Get(id);
        }

        // POST: api/Customer
        [HttpPost]
        public void Post([FromBody]Customer customer)
        {
            _customerGateway.Create(customer);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Customer customer)
        {
            if (id == customer.Id)
            {
                _customerGateway.Update(customer);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _customerGateway.Delete(id);
        }
    }
}

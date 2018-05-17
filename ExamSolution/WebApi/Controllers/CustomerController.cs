using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
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
        private readonly IServiceGateway<Customer> _customerGateway;

        private readonly DbConnection _connection;

        public CustomerController(DbConnection connection)
        {
            this._connection = connection;
            this._customerGateway = new DALFacade().GetCustomerServiceGateway(_connection);
        }
        // GET: api/Customer
        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {

            return await _customerGateway.Get();
        }

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Customer> Get(int id)
        {
            return await _customerGateway.Get(id);
        }

        // POST: api/Customer
        [HttpPost]
        public async Task Post([FromBody] List<Customer> lst)
        {
            foreach (Customer c in lst)
            {
                await _customerGateway.Create(c);
            }
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Customer customer)
        {
            if (id == customer.Id)
            {
                await _customerGateway.Update(customer);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _customerGateway.Delete(id);
        }
    }
}

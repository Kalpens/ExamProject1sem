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
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _customerGateway.Get(id));
            }
            catch
            {
                return BadRequest("Something went wrong while fetching user");
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<Customer> lst)
        {
            try
            {
                foreach (Customer c in lst)
                {
                    await _customerGateway.Create(c);
                }

                return Ok();
            }
            catch
            {
                return BadRequest("Something went wrong while creating user");
            }
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Customer customer)
        {
            if (id == customer.Id)
            {
                return Ok(await _customerGateway.Update(customer));
            }
            else
            {
                return BadRequest("Id Does not match");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _customerGateway.Delete(id);
            if (response)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Something went wrong while deleting user");
            }
        }
    }
}

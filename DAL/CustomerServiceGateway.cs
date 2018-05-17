using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    class CustomerServiceGateway : IServiceGateway<Customer>
    {
        private static CustomerServiceGateway _instance;
        private DbConnection _connection;
        public static CustomerServiceGateway Instance
            => _instance ?? (_instance = new CustomerServiceGateway());

        public void SetDbConnection(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Customer> Get(int id)
        {
            using (var lookupCommand = _connection.CreateCommand())
            {
                lookupCommand.CommandText = @"
                    SELECT * FROM customers Where CustomerId = " + id;
                using (var reader = await lookupCommand.ExecuteReaderAsync())
                {
                    if (reader.Depth < 1)
                    {
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
                    else
                    {
                        throw new Exception("Failed to read customer");
                    }
                }
            }
        }

        public Task<IEnumerable<Customer>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(Customer newObject)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Update(Customer updateObject)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            using (var deleteCommand = _connection.CreateCommand())
            {
                deleteCommand.CommandText = @"
                    DELETE FROM customers Where CustomerId = " + id;
                try
                {
                    await deleteCommand.ExecuteNonQueryAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

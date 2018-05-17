using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
            using (DbCommand lookupCommand = _connection.CreateCommand())
            {
                lookupCommand.CommandText = @"
                    SELECT * FROM customers Where CustomerId = " + id;
                try
                {
                    using (var reader = await lookupCommand.ExecuteReaderAsync())
                    {
                        Customer customer = new Customer();
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
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to fetch user");
                }
            }
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            using (DbCommand lookupCommand = _connection.CreateCommand())
            {
                lookupCommand.CommandText = @"
                    SELECT * FROM customers";
                try
                {
                    using (var reader = await lookupCommand.ExecuteReaderAsync())
                    {
                        List<Customer> listOfCustomers = new List<Customer>();
                        Customer customer = new Customer();
                        while (await reader.ReadAsync())
                        {
                            customer = new Customer();
                            customer.Id = reader.GetInt32(0);
                            customer.BirthDate = reader.GetDateTime(2);
                            customer.Name = reader.GetString(1);
                            customer.PhoneNumber = reader.GetInt32(4);
                            customer.Address = reader.GetString(3);
                            listOfCustomers.Add(customer);
                        }

                        return listOfCustomers;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to read customers");
                }
                        
                }
        }

        public async Task<bool> Create(Customer newObject)
        {
            using (DbCommand createCommand = _connection.CreateCommand())
            {
                createCommand.CommandText = @"insert into customers (Name, Birthdate, Address, Phone) values ('" + newObject.Name +
                    "', '" + newObject.BirthDate.Date.ToString("yyyyMMdd") + "', '" + newObject.Address + "', '" + newObject.PhoneNumber + "');";
                try
                {
                    await createCommand.ExecuteNonQueryAsync();
                }
                catch
                {
                    return false;
                }
                
                //Return true if rows affected is more than zero, otherwise return false
                return true;
            }
        }

        public async Task<Customer> Update(Customer updateObject)
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
                    var resp = await deleteCommand.ExecuteNonQueryAsync();
                    if (resp == 1)
                    {
                        return true;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

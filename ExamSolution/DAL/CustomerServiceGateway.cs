using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using BE;
using MySql.Data.MySqlClient;

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
                        if (reader.HasRows)
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
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to fetch user");
                }
            }
        }

        public async Task<IEnumerable<Customer>> Get(string name)
        {
            using (DbCommand lookupCommand = _connection.CreateCommand())
            {
                lookupCommand.CommandText = @"
                    SELECT * FROM customers Where Name LIKE '%" + name + "%'; ";
                try
                {
                    using (DbDataReader reader = await lookupCommand.ExecuteReaderAsync())
                    {
                        List<Customer> listOfCustomers = new List<Customer>();
                        //Customer customer = new Customer();
                        while (await reader.ReadAsync())
                        {
                            Customer customer = new Customer(
                                (int)reader["CustomerID"], 
                                (string)reader["Name"],
                                (DateTime)reader["Birthdate"], 
                                (string)reader["Address"], 
                                (int)reader["Phone"]);

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

        public async Task<IEnumerable<Customer>> Get()
        {
            using (DbCommand lookupCommand = _connection.CreateCommand())
            {
                lookupCommand.CommandText = @"
                    SELECT * FROM customers;";
                try
                {
                    using (DbDataReader reader = await lookupCommand.ExecuteReaderAsync())
                    {
                        List<Customer> listOfCustomers = new List<Customer>();
                        //Customer customer = new Customer();
                        while (await reader.ReadAsync())
                        {
                            Customer customer = new Customer(
                                (int)reader["CustomerID"], 
                                (string)reader["Name"],
                                (DateTime)reader["Birthdate"], 
                                (string)reader["Address"], 
                                (int)reader["Phone"]);

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
            newObject = new InjectionTest().PrepareCustomer(newObject);
            using (DbCommand createCommand = _connection.CreateCommand())
            {
                createCommand.CommandText = @"insert into customers (Name, Birthdate, Address, Phone) values (@name
                    , @date , @adress , @phoneNum );";
                MySqlParameter nameParam = new MySqlParameter("@name", MySqlDbType.Text);
                MySqlParameter dateParam = new MySqlParameter("@date", MySqlDbType.Date);
                MySqlParameter adressParam = new MySqlParameter("@adress", MySqlDbType.Text);
                MySqlParameter phoneParam = new MySqlParameter("@phoneNum", MySqlDbType.Int16);

                nameParam.Value = newObject.Name;
                dateParam.Value = newObject.BirthDate.Date.ToString("yyyyMMdd");
                adressParam.Value = newObject.Address;
                phoneParam.Value = newObject.PhoneNumber;
                
                createCommand.Parameters.Add(nameParam);
                createCommand.Parameters.Add(dateParam);
                createCommand.Parameters.Add(adressParam);
                createCommand.Parameters.Add(phoneParam);

                // Call Prepare after setting the Commandtext and Parameters.
                createCommand.Prepare();
                try
                {
                    await createCommand.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    return false;
                }
                
                //Return true if rows affected is more than zero, otherwise return false
                return true;
            }
        }

        public async Task<bool> Update(Customer updateObject)
        {
            //Bad way to prevent SQL Injection
            updateObject = new InjectionTest().PrepareCustomer(updateObject);
            using (DbCommand updateCommand = _connection.CreateCommand())
            {
                updateCommand.CommandText = @"update customers set Name='" + updateObject.Name + "', BirthDate='" +
                    updateObject.BirthDate.Date.ToString("yyyMMdd") + "', Address='" + updateObject.Address + "', Phone='" +
                    updateObject.PhoneNumber + "' where CustomerID='" + updateObject.Id + "';";

                try
                {
                    var resp = await updateCommand.ExecuteNonQueryAsync();
                    if (resp == 1)
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }

                return true;
            }
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
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}

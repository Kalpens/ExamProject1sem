using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    public class Customer
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public int? PhoneNumber { get; set; }

        public Customer(int id, string name, DateTime birthdate, string address, int phonenumber)
        {
            Id = id;
            Name = name;
            BirthDate = birthdate;
            Address = address;
            PhoneNumber = phonenumber;
        }

        public Customer()
        {
        }
    }
}

using System;
using System.Collections.Generic;

namespace TestModel
{
    public class Program
    {

    }
}

namespace TestModel.Domain.Customers
{
    public class Customer
    {
        public Customer(Guid id, int age, List<ContactDetails> contacts, Address businessAddress, DateTime createDate, CustomerType type)
        {
            Id = id;
            Age = age;
            Contacts = contacts;
            BusinessAddress = businessAddress;
            CreateDate = createDate;
            Type = type;
        }

        public Guid Id { get; init; }
        public int Age { get; set; }
        public List<ContactDetails> Contacts { get; set; }
        public Address BusinessAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public CustomerType Type { get; set; }
    }

    public class ContactDetails
    {

    }

    public class Address
    {
        public Address(string city, string street)
        {
            City = city;
            Street = street;
        }

        public string City { get; set; }
        public string Street { get; set; }
    }

    public enum CustomerType
    {
        Individual,
        Company
    }
}

namespace TestModel.Domain.Sales
{
    public class Sale
    {
        public Sale(List<Guid> customerId)
        {
            CustomerId = customerId;
        }
        
        public List<Guid> CustomerId { get; set; }
    }
}
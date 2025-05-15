using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    public class CustomerDBrepo : ICustomerRepo
    {
        private readonly AppDBContext _Context;

        public CustomerDBrepo(AppDBContext context)
        {
            _Context = context;
        }
        public Customer GetCustomerById(int id)
        {
            return _Context.Customers.FirstOrDefault(C => C.CustomerId == id);
        }

        public Customer AddCustomer(Customer customer)
        {
            _Context.Customers.Add(customer);
            _Context.SaveChanges();
            return customer;
        }

      

        public Customer? DeleteCustomer(int Id)
        {
            Customer customer = GetCustomerById(Id);
            if (customer != null)
            {
                _Context.Customers.Remove(customer);
                _Context.SaveChanges();
            }
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _Context.Customers;
        }

       

       

        public Customer UpdateCustomer(int id, Customer customer)
        {
            Customer? existing = GetCustomerById(id);
            if (existing == null) 
            {
                return null;
            }
            existing.PhoneNumber = customer.PhoneNumber;
            existing.Email = customer.Email;
            existing.Address = customer.Address;
            existing.CustomerId = customer.CustomerId;
            existing.Name = customer.Name;

            _Context.SaveChanges();
            return existing;
        }
    }
}

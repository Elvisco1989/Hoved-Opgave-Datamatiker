using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public interface ICustomerRepo
    {
        Customer AddCustomer(Customer customer);
       
        Customer DeleteCustomer(int id);
        public IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        public Customer UpdateCustomer(int id, Customer customer);

    }
}
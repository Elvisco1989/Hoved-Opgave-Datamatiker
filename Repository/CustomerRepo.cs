using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private int _nextId = 6;
        private List<Customer> _customers = new List<Customer>
        {
            new Customer{ Name = "Elvis Mbah", Address="Taastrup", CustomerId=1, Email="mbah@ndum", PhoneNumber="45034658", Segment="Monday"},
            new Customer{ Name = "Elvis Ndum", Address="Farum", CustomerId=2, Email="mbah@ndum", PhoneNumber="45034658", Segment="Tuesday"},
            new Customer{ Name = "Elvis Mbah", Address="Copenhagen", CustomerId=3, Email="mbah@ndum", PhoneNumber="45034658", Segment="Wednesday"},
            new Customer{ Name = "Elvis Mbah", Address="Ishøj", CustomerId=4, Email="mbah@ndum", PhoneNumber="45034658", Segment="Thursday"},
            new Customer{ Name = "Elvis Mbah", Address="Roskilde", CustomerId=5, Email="mbah@ndum", PhoneNumber="45034658", Segment="Friday"}

        };

        private List<CustomerDeliveryDates> _links = new List<CustomerDeliveryDates>();





        public Customer AddCustomer(Customer customer)
        {
            customer.CustomerId = ++_nextId;
            _customers.Add(customer);
            return customer;
        }

        public Customer GetCustomerById(int id)
        {
            var customer = _customers.FirstOrDefault(C => C.CustomerId == id);
            return customer;
        }



        public Customer DeleteCustomer(int id)
        {
            Customer customer =  GetCustomerById(id);
            if (customer != null)
            {
                _customers.Remove(customer);
            }
         
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers;
        }

       

        public Customer UpdateCustomer(int id, Customer customer)
        {
            var exitingCustomer = GetCustomerById(id);
            if (exitingCustomer != null)
            {
                exitingCustomer.CustomerId = customer.CustomerId;
                exitingCustomer.PhoneNumber = customer.PhoneNumber;
                exitingCustomer.Address = customer.Address;
                exitingCustomer.Email = customer.Email;
                exitingCustomer.Name = customer.Name;
            }
            return customer;
        }
       
    }
}

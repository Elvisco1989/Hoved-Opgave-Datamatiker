using Hoved_Opgave_Datamatiker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hoved_Opgave_Datamatiker.Repository
{
    /// <summary>
    /// Simpelt in-memory repository til Customer-objekter.
    /// Anvendes typisk til test eller prototyper uden database.
    /// </summary>
    public class CustomerRepo : ICustomerRepo
    {
        // Privat felt til at generere nye unikke CustomerId'er
        private int _nextId = 6;

        // Liste med kunder for at simulere en database
        private List<Customer> _customers = new List<Customer>
        {
            new Customer{ Name = "Elvis", Address="Taastrup", CustomerId=1, Email="mbah@ndum", PhoneNumber="45034658", Segment="Monday"},
            new Customer{ Name = "Elvis Ndum", Address="Farum", CustomerId=2, Email="mbah@ndum", PhoneNumber="45034658", Segment="Tuesday"},
            new Customer{ Name = "Elvis Mbah", Address="Copenhagen", CustomerId=3, Email="mbah@ndum", PhoneNumber="45034658", Segment="Wednesday"},
            new Customer{ Name = "Elvis Mbah", Address="Ishøj", CustomerId=4, Email="mbah@ndum", PhoneNumber="45034658", Segment="Thursday"},
            new Customer{ Name = "Elvis Mbah", Address="Roskilde", CustomerId=5, Email="mbah@ndum", PhoneNumber="45034658", Segment="Friday"}
        };

        // Liste til at simulere relation mellem kunder og leveringsdatoer (ikke brugt i denne kode)
        private List<CustomerDeliveryDates> _links = new List<CustomerDeliveryDates>();

        /// <summary>
        /// Tilføjer en ny kunde til listen.
        /// Tildeler automatisk et nyt unikt CustomerId.
        /// </summary>
        /// <param name="customer">Kundeobjektet der skal tilføjes.</param>
        /// <returns>Den tilføjede kunde med opdateret CustomerId.</returns>
        public Customer AddCustomer(Customer customer)
        {
            customer.CustomerId = ++_nextId;  // Tildel nyt id (auto-increment)
            _customers.Add(customer);          // Tilføj til listen
            return customer;
        }

        /// <summary>
        /// Finder en kunde baseret på CustomerId.
        /// </summary>
        /// <param name="id">Kundens id.</param>
        /// <returns>Kunden, hvis fundet; ellers null.</returns>
        public Customer GetCustomerById(int id)
        {
            var customer = _customers.FirstOrDefault(C => C.CustomerId == id);
            return customer;
        }

        /// <summary>
        /// Sletter en kunde fra listen baseret på CustomerId.
        /// </summary>
        /// <param name="id">Kundens id, der skal slettes.</param>
        /// <returns>Den slettede kunde, eller null hvis ikke fundet.</returns>
        public Customer DeleteCustomer(int id)
        {
            Customer customer = GetCustomerById(id);
            if (customer != null)
            {
                _customers.Remove(customer);
            }

            return customer;
        }

        /// <summary>
        /// Henter alle kunder i listen.
        /// </summary>
        /// <returns>En IEnumerable med alle kunder.</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers;
        }

        /// <summary>
        /// Opdaterer oplysninger på en eksisterende kunde.
        /// </summary>
        /// <param name="id">Id på kunden, der skal opdateres.</param>
        /// <param name="customer">Kundeobjekt med nye værdier.</param>
        /// <returns>Den opdaterede kunde eller null hvis ikke fundet.</returns>
        public Customer UpdateCustomer(int id, Customer customer)
        {
            var existingCustomer = GetCustomerById(id);
            if (existingCustomer != null)
            {
                // Opdater alle relevante felter
                existingCustomer.CustomerId = customer.CustomerId;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.Address = customer.Address;
                existingCustomer.Email = customer.Email;
                existingCustomer.Name = customer.Name;
            }
            return customer;
        }
    }
}

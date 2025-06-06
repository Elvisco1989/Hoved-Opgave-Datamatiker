using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    /// <summary>
    /// Repositoryklasse til håndtering af Customer-entiteter i databasen.
    /// Indeholder CRUD-operationer (Create, Read, Update, Delete) ved brug af Entity Framework Core.
    /// </summary>
    public class CustomerDBrepo : ICustomerRepo
    {
        private readonly AppDBContext _Context;

        /// <summary>
        /// Initialiserer repository med databasekontekst.
        /// </summary>
        /// <param name="context">Instans af AppDBContext til databaseoperationer.</param>
        public CustomerDBrepo(AppDBContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// Finder en kunde baseret på kunde-ID.
        /// </summary>
        /// <param name="id">Kundens ID.</param>
        /// <returns>Customer-objekt eller null hvis ikke fundet.</returns>
        public Customer GetCustomerById(int id)
        {
            return _Context.Customers.FirstOrDefault(C => C.CustomerId == id);
        }

        /// <summary>
        /// Tilføjer en ny kunde til databasen.
        /// </summary>
        /// <param name="customer">Kundeobjektet, der skal tilføjes.</param>
        /// <returns>Det tilføjede kundeobjekt.</returns>
        public Customer AddCustomer(Customer customer)
        {
            _Context.Customers.Add(customer);  // Tilføj kunde til DbSet
            _Context.SaveChanges();            // Gem ændringer i databasen
            return customer;
        }

        /// <summary>
        /// Sletter en kunde baseret på ID.
        /// </summary>
        /// <param name="Id">Kundens ID.</param>
        /// <returns>Det slettede kundeobjekt, eller null hvis ikke fundet.</returns>
        public Customer? DeleteCustomer(int Id)
        {
            Customer customer = GetCustomerById(Id);
            if (customer != null)
            {
                _Context.Customers.Remove(customer); // Fjern kunde fra DbSet
                _Context.SaveChanges();              // Gem ændringer i databasen
            }
            return customer;
        }

        /// <summary>
        /// Henter alle kunder i databasen.
        /// </summary>
        /// <returns>En IEnumerable over alle kunder.</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _Context.Customers;
        }

        /// <summary>
        /// Opdaterer en eksisterende kunde baseret på ID.
        /// </summary>
        /// <param name="id">ID på kunden, der skal opdateres.</param>
        /// <param name="customer">Objekt med opdaterede kundeoplysninger.</param>
        /// <returns>Det opdaterede kundeobjekt, eller null hvis kunden ikke findes.</returns>
        public Customer UpdateCustomer(int id, Customer customer)
        {
            Customer? existing = GetCustomerById(id);
            if (existing == null)
            {
                return null;
            }

            // Opdater felter
            existing.PhoneNumber = customer.PhoneNumber;
            existing.Email = customer.Email;
            existing.Address = customer.Address;
            existing.CustomerId = customer.CustomerId; // Overvej om denne skal opdateres, da ID normalt ikke ændres
            existing.Name = customer.Name;

            _Context.SaveChanges(); // Gem ændringer i databasen
            return existing;
        }
    }
}

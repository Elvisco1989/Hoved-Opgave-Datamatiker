// Importerer modellen Customer, som bruges i metoderne.
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository // Definerer namespace for repositories.
{
    /// <summary>
    /// Interface for repository der håndterer CRUD-operationer for kunder.
    /// </summary>
    public interface ICustomerRepo
    {
        /// <summary>
        /// Tilføjer en ny kunde til systemet.
        /// </summary>
        /// <param name="customer">Kunden, der skal tilføjes.</param>
        /// <returns>Den tilføjede kunde.</returns>
        Customer AddCustomer(Customer customer);

        /// <summary>
        /// Sletter en kunde baseret på deres ID.
        /// </summary>
        /// <param name="id">ID på kunden, der skal slettes.</param>
        /// <returns>Den slettede kunde, eller null hvis ikke fundet.</returns>
        Customer DeleteCustomer(int id);

        /// <summary>
        /// Henter alle kunder i systemet.
        /// </summary>
        /// <returns>En liste over alle kunder.</returns>
        public IEnumerable<Customer> GetAllCustomers();

        /// <summary>
        /// Henter en enkelt kunde ud fra ID.
        /// </summary>
        /// <param name="id">ID på den ønskede kunde.</param>
        /// <returns>Kunden med det angivne ID, eller null hvis ikke fundet.</returns>
        Customer GetCustomerById(int id);

        /// <summary>
        /// Opdaterer en eksisterende kunde.
        /// </summary>
        /// <param name="id">ID på kunden, der skal opdateres.</param>
        /// <param name="customer">De opdaterede kundeoplysninger.</param>
        /// <returns>Den opdaterede kunde.</returns>
        public Customer UpdateCustomer(int id, Customer customer);
    }
}

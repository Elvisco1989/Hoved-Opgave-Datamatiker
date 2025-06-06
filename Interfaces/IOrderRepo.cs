using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    /// <summary>
    /// Interface for repository til håndtering af ordrer.
    /// Definerer CRUD-operationer for Order-entiteten.
    /// </summary>
    public interface IOrderRepo
    {
        /// <summary>
        /// Tilføjer en ny ordre til databasen.
        /// </summary>
        /// <param name="order">Den ordre der skal tilføjes.</param>
        /// <returns>Den tilføjede ordre med genereret ID.</returns>
        Order AddOrder(Order order);

        /// <summary>
        /// Henter en ordre baseret på dens ID.
        /// </summary>
        /// <param name="id">Ordre-ID.</param>
        /// <returns>Ordren, hvis den findes; ellers null.</returns>
        Order GetOrderid(int id);

        /// <summary>
        /// Returnerer en liste over alle ordrer i systemet.
        /// </summary>
        /// <returns>Enumerable af alle ordrer.</returns>
        IEnumerable<Order> GetOrders();

        /// <summary>
        /// Opdaterer en eksisterende ordre.
        /// </summary>
        /// <param name="order">Ordren med opdaterede data.</param>
        void UpdateOrder(Order order);

        /// <summary>
        /// Sletter en ordre baseret på dens ID.
        /// </summary>
        /// <param name="id">Ordre-ID.</param>
        /// <returns>Den slettede ordre, hvis den fandtes; ellers null.</returns>
        Order? DeleteOrder(int id);
    }
}

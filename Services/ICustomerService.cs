using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Interface for kundeservice, som håndterer funktioner relateret til kunder.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Metode til at tilknytte leveringsdatoer til en kunde.
        /// </summary>
        /// <param name="customer">Kunden der skal have tildelt leveringsdatoer</param>
        /// <param name="deliveryDates">Liste af leveringsdatoer</param>
        void AssignDeliveryDates(Customer customer, List<DeliveryDates> deliveryDates);

        /// <summary>
        /// Henter en liste af leveringsdatoer tilknyttet en bestemt kunde.
        /// </summary>
        /// <param name="customerId">Kundens id</param>
        /// <returns>Liste af CustomerDeliveryDateDto objekter</returns>
        List<CustomerDeliveryDateDto> GetDeliveryDatesForCustomer(int customerId);
    }
}

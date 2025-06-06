using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Interfaces;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Service til at håndtere kunde-relaterede operationer, fx tildeling af leveringsdatoer.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly AppDBContext _context;

        public CustomerService(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tildeler en liste af leveringsdatoer til en kunde ved at oprette forbindelser i databasen.
        /// </summary>
        /// <param name="customer">Kunden der skal have tilknyttet leveringsdatoer</param>
        /// <param name="deliveryDates">Listen af leveringsdatoer</param>
        public void AssignDeliveryDates(Customer customer, List<DeliveryDates> deliveryDates)
        {
            foreach (var date in deliveryDates)
            {
                var link = new CustomerDeliveryDates
                {
                    CustomerId = customer.CustomerId,
                    DeliveryDateId = date.DeliveryDateId
                };

                _context.CustomerDeliveryDates.Add(link);
            }

            _context.SaveChanges(); // Gemmer ændringerne i databasen
        }

        /// <summary>
        /// Henter leveringsdatoerne for en bestemt kunde som DTO'er.
        /// </summary>
        /// <param name="customerId">Kundens ID</param>
        /// <returns>Liste af CustomerDeliveryDateDto med leveringsdatoer</returns>
        public List<CustomerDeliveryDateDto> GetDeliveryDatesForCustomer(int customerId)
        {
            return _context.CustomerDeliveryDates
                .Where(l => l.CustomerId == customerId)
                .Include(l => l.DeliveryDate) // Inkluder relaterede leveringsdatoer
                .Select(l => new CustomerDeliveryDateDto
                {
                    DeliveryDate = l.DeliveryDate.DeliveryDate
                })
                .ToList();
        }
    }
}

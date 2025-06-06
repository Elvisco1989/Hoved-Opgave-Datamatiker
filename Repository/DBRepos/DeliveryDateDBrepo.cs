using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    /// <summary>
    /// Repository implementation for managing DeliveryDates using the database context (EF Core).
    /// </summary>
    public class DeliveryDateDBrepo : IDeliveryDateRepo
    {
        /// <summary>
        /// Database context used for accessing delivery dates.
        /// </summary>
        private readonly AppDBContext _context;

        /// <summary>
        /// Constructor injecting the database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public DeliveryDateDBrepo(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new delivery date to the database.
        /// </summary>
        /// <param name="deliveryDate">The delivery date to add.</param>
        /// <returns>The added delivery date with database-generated ID.</returns>
        public DeliveryDates AddDeliveryDate(DeliveryDates deliveryDate)
        {
            _context.DeliveryDates.Add(deliveryDate);
            _context.SaveChanges();
            return deliveryDate;
        }

        /// <summary>
        /// Deletes a delivery date from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery date to delete.</param>
        public void DeleteDeliveryDate(int id)
        {
            var deliveryDate = _context.DeliveryDates.Find(id);
            if (deliveryDate != null)
            {
                _context.DeliveryDates.Remove(deliveryDate);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves all delivery dates from the database, ordered by date.
        /// </summary>
        /// <returns>List of delivery dates.</returns>
        public List<DeliveryDates> GetAllDeliveryDates()
        {
            return _context.DeliveryDates.OrderBy(d => d.DeliveryDate).ToList();
        }

        /// <summary>
        /// Retrieves a specific delivery date by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery date to retrieve.</param>
        /// <returns>The matching delivery date, or null if not found.</returns>
        public DeliveryDates GetDeliveryDateById(int id)
        {
            return _context.DeliveryDates.Find(id);
        }

        /// <summary>
        /// Updates an existing delivery date if it exists.
        /// </summary>
        /// <param name="deliveryDate">The delivery date with updated values.</param>
        public void UpdateDeliveryDate(DeliveryDates deliveryDate)
        {
            var existingDate = _context.DeliveryDates.Find(deliveryDate.DeliveryDateId);
            if (existingDate != null)
            {
                existingDate.DeliveryDate = deliveryDate.DeliveryDate;
                _context.SaveChanges();
            }
        }
    }
}


using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    /// <summary>
    /// Repository for managing delivery dates.
    /// Uses in-memory storage for demonstration or testing purposes.
    /// </summary>
    public class DeliveryDateRepo : IDeliveryDateRepo
    {
        /// <summary>
        /// Internal list simulating the storage of delivery dates.
        /// </summary>
        private List<DeliveryDates> _deliveryDates = new List<DeliveryDates>();

        /// <summary>
        /// Counter used for generating unique DeliveryDateIds.
        /// </summary>
        private int _nextId = 1;

        /// <summary>
        /// Constructor initializes the repository with both fixed and generated delivery dates.
        /// </summary>
        public DeliveryDateRepo()
        {
            // Add 5 fixed mock delivery dates
            _deliveryDates = new List<DeliveryDates>
            {
                new DeliveryDates { DeliveryDateId = _nextId++, DeliveryDate = new DateTime(2025, 5, 8) },
                new DeliveryDates { DeliveryDateId = _nextId++, DeliveryDate = new DateTime(2025, 5, 9) },
                new DeliveryDates { DeliveryDateId = _nextId++, DeliveryDate = new DateTime(2025, 5, 10) },
                new DeliveryDates { DeliveryDateId = _nextId++, DeliveryDate = new DateTime(2025, 5, 11) },
                new DeliveryDates { DeliveryDateId = _nextId++, DeliveryDate = new DateTime(2025, 5, 12) },
            };

            // Then add delivery dates for the next 30 days
            var today = DateTime.Today;
            for (int i = 0; i < 30; i++)
            {
                var date = today.AddDays(i);
                _deliveryDates.Add(new DeliveryDates
                {
                    DeliveryDateId = _nextId++,
                    DeliveryDate = date
                });
            }
        }

        /// <summary>
        /// Retrieves a list of delivery dates matching the given segment (day of week).
        /// </summary>
        /// <param name="segment">The delivery segment (enum based on DayOfWeek).</param>
        /// <param name="count">The maximum number of delivery dates to return.</param>
        /// <returns>List of delivery dates matching the segment.</returns>
        public List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count)
        {
            DayOfWeek targetDay = (DayOfWeek)segment;
            return _deliveryDates
                .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
                .OrderBy(d => d.DeliveryDate)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Retrieves all delivery dates stored in the repository.
        /// </summary>
        /// <returns>List of all delivery dates.</returns>
        public List<DeliveryDates> GetAllDeliveryDates()
        {
            return _deliveryDates;
        }

        /// <summary>
        /// Retrieves a delivery date by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the delivery date to retrieve.</param>
        /// <returns>The matching DeliveryDates object, or null if not found.</returns>
        public DeliveryDates GetDeliveryDateById(int id)
        {
            return _deliveryDates.FirstOrDefault(d => d.DeliveryDateId == id);
        }

        /// <summary>
        /// Adds a new delivery date to the repository.
        /// </summary>
        /// <param name="deliveryDate">The delivery date to add.</param>
        /// <returns>The newly added delivery date with assigned ID.</returns>
        public DeliveryDates AddDeliveryDate(DeliveryDates deliveryDate)
        {
            deliveryDate.DeliveryDateId = _nextId++;
            _deliveryDates.Add(deliveryDate);
            return deliveryDate;
        }

        /// <summary>
        /// Updates an existing delivery date if it exists.
        /// </summary>
        /// <param name="deliveryDate">The delivery date with updated values.</param>
        public void UpdateDeliveryDate(DeliveryDates deliveryDate)
        {
            var existingDate = GetDeliveryDateById(deliveryDate.DeliveryDateId);
            if (existingDate != null)
            {
                existingDate.DeliveryDate = deliveryDate.DeliveryDate;
            }
        }

        /// <summary>
        /// Deletes a delivery date by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery date to delete.</param>
        public void DeleteDeliveryDate(int id)
        {
            var deliveryDate = GetDeliveryDateById(id);
            if (deliveryDate != null)
            {
                _deliveryDates.Remove(deliveryDate);
            }
        }
    }
}


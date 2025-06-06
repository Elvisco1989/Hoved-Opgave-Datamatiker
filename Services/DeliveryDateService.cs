using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Service for handling delivery dates, including segment-based logic and dynamic date generation.
    /// </summary>
    public class DeliveryDateService : IDeliveryDateService
    {
        /// <summary>
        /// Database context used for accessing delivery dates.
        /// </summary>
        private readonly AppDBContext _Context;

        /// <summary>
        /// In-memory list used only by the non-database method.
        /// </summary>
        private List<DeliveryDates> _deliveryDates = new List<DeliveryDates>();

        /// <summary>
        /// Constructor injecting the database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public DeliveryDateService(AppDBContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// Retrieves delivery dates from the in-memory list for a specific segment (weekday).
        /// </summary>
        /// <param name="segment">The delivery segment (mapped from DayOfWeek).</param>
        /// <param name="count">Maximum number of delivery dates to return.</param>
        /// <returns>List of delivery dates for the specified segment.</returns>
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
        /// Retrieves delivery dates for a segment directly from the database.
        /// If not enough dates exist, generates and inserts new dates for that segment.
        /// </summary>
        /// <param name="segment">The delivery segment (mapped from DayOfWeek).</param>
        /// <param name="count">Desired number of delivery dates to retrieve or generate.</param>
        /// <returns>List of delivery dates for the specified segment.</returns>
        public List<DeliveryDates> GetDeliveryDatesForSegmentDB(Segment segment, int count)
        {
            DayOfWeek targetDay = (DayOfWeek)segment;
            var today = DateTime.Today;

            // First load existing delivery dates for the given segment
            var existingDates = _Context.DeliveryDates
                .Where(d => d.DeliveryDate >= today)
                .AsEnumerable()
                .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
                .OrderBy(d => d.DeliveryDate)
                .ToList();

            int toGenerate = count - existingDates.Count;

            if (toGenerate > 0)
            {
                var newDates = new List<DeliveryDates>();
                int daysAhead = 0;
                int found = 0;

                // Generate missing delivery dates dynamically
                while (found < toGenerate && daysAhead < 365) // safety limit of 1 year
                {
                    var candidateDate = today.AddDays(daysAhead);
                    if (candidateDate.DayOfWeek == targetDay &&
                        !_Context.DeliveryDates.Any(d => d.DeliveryDate == candidateDate))
                    {
                        newDates.Add(new DeliveryDates { DeliveryDate = candidateDate });
                        found++;
                    }

                    daysAhead++;
                }

                if (newDates.Any())
                {
                    _Context.DeliveryDates.AddRange(newDates);
                    _Context.SaveChanges();
                }

                // Refresh list after generating new dates
                existingDates = _Context.DeliveryDates
                    .Where(d => d.DeliveryDate >= today)
                    .AsEnumerable()
                    .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
                    .OrderBy(d => d.DeliveryDate)
                    .Take(count)
                    .ToList();
            }
            else
            {
                existingDates = existingDates.Take(count).ToList();
            }

            return existingDates;
        }
    }
}

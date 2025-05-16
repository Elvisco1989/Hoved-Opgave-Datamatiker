using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Services
{
    public class DeliveryDateService : IDeliveryDateService
    {
        private readonly AppDBContext _Context;
        private List<DeliveryDates> _deliveryDates = new List<DeliveryDates>();

        public DeliveryDateService(AppDBContext context)
        {
            _Context = context;
        }

        public List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count)
        {
            DayOfWeek targetDay = (DayOfWeek)segment;
            return _deliveryDates
                .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
                .OrderBy(d => d.DeliveryDate)
                .Take(count)
                .ToList();
        }

        public List<DeliveryDates> GetDeliveryDatesForSegmentDB(Segment segment, int count)
        {
            DayOfWeek targetDay = (DayOfWeek)segment;
            var today = DateTime.Today;

            // First filter only on what EF can translate (e.g., DeliveryDate >= today)
            var existingDates = _Context.DeliveryDates
                .Where(d => d.DeliveryDate >= today)
                .AsEnumerable() // switch to client-side LINQ
                .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
                .OrderBy(d => d.DeliveryDate)
                .ToList();

            int toGenerate = count - existingDates.Count;

            if (toGenerate > 0)
            {
                var newDates = new List<DeliveryDates>();
                int daysAhead = 0;
                int found = 0;

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

                // Refresh the list after adding new dates
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

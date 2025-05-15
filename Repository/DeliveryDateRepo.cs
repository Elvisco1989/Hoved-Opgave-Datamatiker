using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public class DeliveryDateRepo : IDeliveryDateRepo
    {
        private List<DeliveryDates> _deliveryDates = new List<DeliveryDates>();
        private int _nextId = 1;

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


        public List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count)
        {
            DayOfWeek targetDay = (DayOfWeek)segment;
            return _deliveryDates
                .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
                .OrderBy(d => d.DeliveryDate)
                .Take(count)
                .ToList();
        }

        public List<DeliveryDates> GetAllDeliveryDates()
        {
            return _deliveryDates;
        }

        public DeliveryDates GetDeliveryDateById(int id)
        {
            return _deliveryDates.FirstOrDefault(d => d.DeliveryDateId == id);
        }

        public DeliveryDates AddDeliveryDate(DeliveryDates deliveryDate)
        {
            deliveryDate.DeliveryDateId = _nextId++;
            _deliveryDates.Add(deliveryDate);
            return deliveryDate;
        }

        public void UpdateDeliveryDate(DeliveryDates deliveryDate)
        {
            var existingDate = GetDeliveryDateById(deliveryDate.DeliveryDateId);
            if (existingDate != null)
            {
                existingDate.DeliveryDate = deliveryDate.DeliveryDate;
            }
        }

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

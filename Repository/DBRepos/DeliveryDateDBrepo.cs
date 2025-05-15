using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    public class DeliveryDateDBrepo : IDeliveryDateRepo
    {
        private readonly AppDBContext _context;

        public DeliveryDateDBrepo(AppDBContext context)
        {
            _context = context;
        }

        public DeliveryDates AddDeliveryDate(DeliveryDates deliveryDate)
        {
            _context.DeliveryDates.Add(deliveryDate);
            _context.SaveChanges();
            return deliveryDate;
        }

        public void DeleteDeliveryDate(int id)
        {
            var deliveryDate = _context.DeliveryDates.Find(id);
            if (deliveryDate != null)
            {
                _context.DeliveryDates.Remove(deliveryDate);
                _context.SaveChanges();
            }
        }

        public List<DeliveryDates> GetAllDeliveryDates()
        {
            return _context.DeliveryDates.OrderBy(d => d.DeliveryDate).ToList();
        }

        public DeliveryDates GetDeliveryDateById(int id)
        {
            return _context.DeliveryDates.Find(id);
        }

        //public List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count)
        //{
        //    DayOfWeek targetDay = (DayOfWeek)segment;
        //    return _context.DeliveryDates
        //                   .Where(d => d.DeliveryDate.DayOfWeek == targetDay)
        //                   .OrderBy(d => d.DeliveryDate)
        //                   .Take(count)
        //                   .ToList();
        //}

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

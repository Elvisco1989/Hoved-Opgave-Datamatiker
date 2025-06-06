using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    public interface IDeliveryDateService
    {
        List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count);
        public List<DeliveryDates> GetDeliveryDatesForSegmentDB(Segment segment, int count);
    }
}
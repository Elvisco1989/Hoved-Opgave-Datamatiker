using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Interfaces
{
    public interface IDeliveryDate
    {
        public DeliveryDates GetDeliveryDate();

        public DeliveryDates AddDeliveryDate(DeliveryDates deliveryDates);

        public DeliveryDates RemoveDeliveryDate(DeliveryDates deliveryDates);

        public DeliveryDates UpdateDeliveryDates(DeliveryDates deliveryDates);

        public List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count);



    }
}

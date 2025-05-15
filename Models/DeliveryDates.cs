using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    public class DeliveryDates
    {
        [Key]
        public int DeliveryDateId { get; set; }
        public DateTime DeliveryDate { get; set; }

        public int customerId { get; set; } = 0;

        public List<CustomerDeliveryDates> CustomerDeliveryDates { get; set; } = new List<CustomerDeliveryDates>();


    }
}

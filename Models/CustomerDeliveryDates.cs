using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    public class CustomerDeliveryDates
    {

        [Key]
        public int CustomerId { get; set; }

        [Key]

        public int DeliveryDateId { get; set; }

        public Customer Customer { get; set; }

      
        public DeliveryDates DeliveryDate { get; internal set; }
    }
}

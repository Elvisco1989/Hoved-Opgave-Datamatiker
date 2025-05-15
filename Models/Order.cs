using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int customerId { get; set; }

        public Customer Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();

        public int UnitPrice { get; set; } // Price in cents

        public decimal TotalAmount { get; set; } // In dollars or your default currency
        public string PaymentStatus { get; set; } = "Pending"; // "Pending", "Paid", "Failed"

        public string? StripePaymentIntentId { get; set; } // <- store Stripe ID here
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}

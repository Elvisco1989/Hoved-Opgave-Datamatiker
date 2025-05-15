using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    public class OrderItem
    {
      
        public int OrderId { get; set; }
       
        public int ProductId { get; set; }
        public Order Order { get; set; }
        
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; } // Price in cents
    }
}

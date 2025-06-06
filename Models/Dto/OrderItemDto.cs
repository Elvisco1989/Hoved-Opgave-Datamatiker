namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductDto Product { get; set; }

        public int UnitPrice { get; set; } // Price in cents
    }
}

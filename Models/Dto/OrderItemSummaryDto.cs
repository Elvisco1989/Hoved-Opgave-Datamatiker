namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class OrderItemSummaryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

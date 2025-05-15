namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class ProductOrderSummaryDto
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Total => Price * Quantity;
    }
}

namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class OrderSumDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime NextDeliveryDate { get; set; }
        public int OrderId { get; set; }
        public List<ProductOrderSummaryDto> Products { get; set; }
        public int OrderTotal => Products.Sum(p => p.Total);
    }
}

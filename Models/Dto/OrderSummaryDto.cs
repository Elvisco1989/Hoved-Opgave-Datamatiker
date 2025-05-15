namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public List<OrderItemSummaryDto> OrderItems { get; set; } = new();
    }
}

namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}

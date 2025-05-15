namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}

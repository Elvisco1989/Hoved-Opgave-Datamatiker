namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Segment { get; set; }
        public List<DateTime> DeliveryDates { get; set; } = new();
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}

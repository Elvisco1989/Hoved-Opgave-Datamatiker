namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    public class TopSellingProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantitySold { get; set; }
    }
}

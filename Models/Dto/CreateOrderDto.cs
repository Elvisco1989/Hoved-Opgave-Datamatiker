namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) til oprettelse af en ny ordre.
    /// Indeholder oplysninger om kunden og de produkter, der bestilles.
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// ID på den kunde, der placerer ordren.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Liste over produkter og mængder, der skal inkluderes i ordren.
        /// </summary>
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}

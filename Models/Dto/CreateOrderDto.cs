namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace til DTO-klasser.
{
    /// <summary>
    /// DTO til oprettelse af en ny ordre.
    /// Indeholder oplysninger om kunden og de produkter, der bestilles.
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
        /// ID på kunden, som ordren skal tilknyttes.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Liste over varer, der skal inkluderes i ordren.
        /// Hver vare beskrives via en <see cref="CreateOrderItemDto"/>.
        /// </summary>
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}

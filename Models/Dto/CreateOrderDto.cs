namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace til DTO-klasser.
{
    /// <summary>
<<<<<<< HEAD
    /// DTO til oprettelse af en ny ordre.
=======
    /// Data Transfer Object (DTO) til oprettelse af en ny ordre.
>>>>>>> 96b5ca0fa3cc08fe13708d7fd00ad29cda7995b3
    /// Indeholder oplysninger om kunden og de produkter, der bestilles.
    /// </summary>
    public class CreateOrderDto
    {
        /// <summary>
<<<<<<< HEAD
        /// ID på kunden, som ordren skal tilknyttes.
=======
        /// ID på den kunde, der placerer ordren.
>>>>>>> 96b5ca0fa3cc08fe13708d7fd00ad29cda7995b3
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
<<<<<<< HEAD
        /// Liste over varer, der skal inkluderes i ordren.
        /// Hver vare beskrives via en <see cref="CreateOrderItemDto"/>.
=======
        /// Liste over produkter og mængder, der skal inkluderes i ordren.
>>>>>>> 96b5ca0fa3cc08fe13708d7fd00ad29cda7995b3
        /// </summary>
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}

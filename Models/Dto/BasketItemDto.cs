namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace for Data Transfer Objects (DTOs).
{
    /// <summary>
    /// Data Transfer Object (DTO) til at overføre information om et kurv-element.
    /// Bruges når data sendes mellem klient og server (f.eks. via API).
    /// </summary>
    public class BasketItemDto
    {
        // Denne linje er udkommenteret og bruges muligvis ikke i nuværende implementering.
        // Hvis du ønsker at håndtere flere kunders kurve, kan det være relevant at inkludere CustomerId.
        // public int CustomerId { get; set; }

        /// <summary>
        /// ID for det produkt, som skal tilføjes til eller opdateres i kurven.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Antallet af produkter der ønskes tilføjet eller opdateret i kurven.
        /// </summary>
        public int Quantity { get; set; }
    }
}

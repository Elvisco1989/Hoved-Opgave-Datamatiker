using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    /// <summary>
    /// Modelklasse, der repræsenterer et produkt i systemet.
    /// Indeholder oplysninger om produktets navn, beskrivelse, pris, lagerstatus, billede og tilknyttede ordrelinjer.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unikt ID for produktet (primærnøgle).
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Produktets navn. Obligatorisk felt.
        /// Maksimum 100 tegn.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Beskrivelse af produktet. Obligatorisk felt.
        /// Maksimum 100 tegn.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Produktets pris. Obligatorisk felt.
        /// Bruger decimal-type til at håndtere præcise monetære værdier.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Lagerstatus - hvor mange produkter der er på lager.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Sti eller filnavn til billedfil, som repræsenterer produktet.
        /// Maksimum 200 tegn.
        /// </summary>
        [StringLength(200)]
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Navigationsegenskab der repræsenterer de ordrelinjer, hvor dette produkt indgår.
        /// </summary>
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}

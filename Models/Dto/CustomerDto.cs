namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace til data transfer objects.
{
    /// <summary>
    /// DTO der repræsenterer en kunde og dens relevante oplysninger, 
    /// inklusiv leveringsdatoer og tilknyttede ordrer.
    /// Bruges typisk til at sende kundeinformation ud via API.
    /// </summary>
    public class CustomerDto
    {
        /// <summary>
        /// Unikt ID for kunden.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Kundens fulde navn.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kundens adresse.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Segment som kunden tilhører, f.eks. "Monday", "Zone A", osv.
        /// Bruges til ruteplanlægning eller gruppering.
        /// </summary>
        public string Segment { get; set; }

        /// <summary>
        /// Liste over kundens planlagte leveringsdatoer.
        /// </summary>
        public List<DateTime> DeliveryDates { get; set; } = new();

        /// <summary>
        /// Liste over ordrer foretaget af kunden.
        /// Hver ordre er repræsenteret som et <see cref="OrderDto"/>.
        /// </summary>
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}

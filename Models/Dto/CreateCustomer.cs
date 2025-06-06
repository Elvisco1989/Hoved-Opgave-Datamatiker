namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace for data transfer objects (DTOs).
{
    /// <summary>
    /// DTO til oprettelse af en ny kunde.
    /// Indeholder alle nødvendige oplysninger for at registrere kunden i systemet.
    /// </summary>
    public class CreateCustomer
    {
        /// <summary>
        /// Kundens fulde navn.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kundens adresse.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Kundens telefonnummer.
        /// Initialiseres som tom streng for at undgå null.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Kundens e-mailadresse.
        /// Initialiseres som tom streng for at undgå null.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Liste over ønskede leveringsdatoer for kunden.
        /// Bruges f.eks. til at planlægge fremtidige ordrer.
        /// </summary>
        public List<DateTime> DeliveryDates { get; set; } = new();

        /// <summary>
        /// Kundens segment (f.eks. baseret på ugedag for levering: "Monday", "Tuesday", osv.).
        /// Segment kan også bruges til at gruppere kunder i ruter eller tidszoner.
        /// </summary>
        public string Segment { get; set; }
    }
}

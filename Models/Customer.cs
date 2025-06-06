using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    /// <summary>
    /// Repræsenterer en kunde med grundlæggende oplysninger, 
    /// kontaktinformation, segment og relationer til leveringsdatoer og ordrer.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unik identifikator for kunden.
        /// </summary>
        [Key]
        public int CustomerId { get; set; }

        /// <summary>
        /// Kundens navn. Maksimum 100 tegn.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kundens adresse. Maksimum 100 tegn.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Kundens telefonnummer.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Segment kunden tilhører (f.eks. "Mandag", "Tirsdag").
        /// </summary>
        [Required]
        public string Segment { get; set; } = string.Empty;

        /// <summary>
        /// Kundens emailadresse (valideres).
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Fremmednøgle til leveringsdato (kan være 0 hvis ikke sat).
        /// </summary>
        public int DeliveryDateId { get; set; } = 0;

        /// <summary>
        /// Liste over relationer mellem kunden og leveringsdatoer (many-to-many).
        /// </summary>
        public List<CustomerDeliveryDates> CustomerDeliveryDates { get; set; } = new List<CustomerDeliveryDates>();

        /// <summary>
        /// Liste over ordrer tilknyttet kunden (one-to-many).
        /// </summary>
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}

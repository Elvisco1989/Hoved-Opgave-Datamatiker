using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    /// <summary>
    /// Repræsenterer en leveringsdato, som kan være knyttet til en eller flere kunder.
    /// </summary>
    public class DeliveryDates
    {
        /// <summary>
        /// Entydigt ID for leveringsdatoen.
        /// </summary>
        [Key]
        public int DeliveryDateId { get; set; }

        /// <summary>
        /// Selve datoen for levering.
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// (Valgfri) ID på en specifik kunde, hvis datoen kun gælder for én kunde.
        /// Som standard 0 hvis ikke specifik.
        /// </summary>
        public int customerId { get; set; } = 0;

        /// <summary>
        /// Navigation property der angiver de kunder, som denne leveringsdato er knyttet til.
        /// Bruges i mange-til-mange-relation med Customer.
        /// </summary>
        public List<CustomerDeliveryDates> CustomerDeliveryDates { get; set; } = new List<CustomerDeliveryDates>();
    }
}

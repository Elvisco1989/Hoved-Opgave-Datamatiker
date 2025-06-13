using System.ComponentModel.DataAnnotations;

namespace Hoved_Opgave_Datamatiker.Models
{
    /// <summary>
    /// Repræsenterer forbindelsen mellem en kunde og en leveringsdato.
    /// Bruges som join-tabel i en mange-til-mange-relation mellem Customer og DeliveryDates.
    /// </summary>
    public class CustomerDeliveryDates
    {
        /// <summary>
        /// ID på kunden. Primær nøgle sammen med DeliveryDateId.
        /// </summary>
        [Key]
        public int CustomerId { get; set; }

        /// <summary>
        /// ID på leveringsdatoen. Primær nøgle sammen med CustomerId.
        /// </summary>
        [Key]
        public int DeliveryDateId { get; set; }

        /// <summary>
        /// Navigation property til den tilknyttede kunde.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Navigation property til den tilknyttede leveringsdato.
        /// </summary>
        public DeliveryDates DeliveryDate { get; internal set; }
    }
}

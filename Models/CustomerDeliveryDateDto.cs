namespace Hoved_Opgave_Datamatiker.Models
{
    /// <summary>
    /// Data Transfer Object (DTO), der repræsenterer en leveringsdato for en kunde.
    /// Bruges typisk til at vise eller sende leveringsdatoer relateret til en kundes ordre.
    /// </summary>
    public class CustomerDeliveryDateDto
    {
        /// <summary>
        /// Datoen hvor levering er planlagt til at finde sted.
        /// </summary>
        public DateTime DeliveryDate { get; set; }
    }
}

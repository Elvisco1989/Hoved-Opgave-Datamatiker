// Importerer modellen DeliveryDates og Segment.
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Interfaces // Definerer namespace for interfaces.
{
    /// <summary>
    /// Interface til håndtering af leveringsdatoer i systemet.
    /// </summary>
    public interface IDeliveryDate
    {
        /// <summary>
        /// Henter en leveringsdato.
        /// </summary>
        /// <returns>En instans af <see cref="DeliveryDates"/>.</returns>
        public DeliveryDates GetDeliveryDate();

        /// <summary>
        /// Tilføjer en ny leveringsdato til systemet.
        /// </summary>
        /// <param name="deliveryDates">Objektet der repræsenterer leveringsdatoen.</param>
        /// <returns>Den tilføjede <see cref="DeliveryDates"/>.</returns>
        public DeliveryDates AddDeliveryDate(DeliveryDates deliveryDates);

        /// <summary>
        /// Fjerner en eksisterende leveringsdato.
        /// </summary>
        /// <param name="deliveryDates">Objektet der skal fjernes.</param>
        /// <returns>Den fjernede <see cref="DeliveryDates"/>.</returns>
        public DeliveryDates RemoveDeliveryDate(DeliveryDates deliveryDates);

        /// <summary>
        /// Opdaterer en eksisterende leveringsdato.
        /// </summary>
        /// <param name="deliveryDates">Den opdaterede leveringsdato.</param>
        /// <returns>Den opdaterede <see cref="DeliveryDates"/>.</returns>
        public DeliveryDates UpdateDeliveryDates(DeliveryDates deliveryDates);

        /// <summary>
        /// Henter en liste med leveringsdatoer baseret på segment.
        /// </summary>
        /// <param name="segment">Segmentet som leveringsdatoerne relaterer sig til.</param>
        /// <param name="count">Antal leveringsdatoer der ønskes.</param>
        /// <returns>En liste af <see cref="DeliveryDates"/> for det angivne segment.</returns>
        public List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count);
    }
}

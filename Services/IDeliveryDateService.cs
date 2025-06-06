using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Interface defining delivery date service functionality.
    /// </summary>
    public interface IDeliveryDateService
    {
        /// <summary>
        /// Retrieves delivery dates for a specified segment from in-memory data.
        /// </summary>
        /// <param name="segment">The delivery segment (mapped from DayOfWeek).</param>
        /// <param name="count">The maximum number of delivery dates to return.</param>
        /// <returns>List of delivery dates matching the specified segment.</returns>
        List<DeliveryDates> GetDeliveryDatesForSegment(Segment segment, int count);

        /// <summary>
        /// Retrieves delivery dates for a segment from the database and dynamically generates new dates if necessary.
        /// </summary>
        /// <param name="segment">The delivery segment (mapped from DayOfWeek).</param>
        /// <param name="count">The desired number of delivery dates to retrieve.</param>
        /// <returns>List of delivery dates matching the specified segment.</returns>
        public List<DeliveryDates> GetDeliveryDatesForSegmentDB(Segment segment, int count);
    }
}

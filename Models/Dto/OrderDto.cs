namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace til data transfer objects.
{
    /// <summary>
    /// DTO der repræsenterer en ordre.
    /// Indeholder ordre-id, tilhørende kunde-id og en liste over ordrelinjer.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Unikt ID for ordren.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// ID for kunden, der har afgivet ordren.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Liste over ordrelinjer i ordren.
        /// Hver linje er repræsenteret som et <see cref="OrderItemDto"/>.
        /// </summary>
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}

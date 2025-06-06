namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    /// <summary>
    /// DTO der opsummerer en produkts ordreinformation.
    /// Indeholder produktnavn, pris per enhed, bestilt antal og total pris.
    /// </summary>
    public class ProductOrderSummaryDto
    {
        /// <summary>
        /// Navnet på produktet.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Prisen pr. enhed i hele enheder (f.eks. øre eller cents).
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Antal af produkter bestilt.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Totalprisen for produkterne, beregnet som Price * Quantity.
        /// </summary>
        public int Total => Price * Quantity;
    }
}

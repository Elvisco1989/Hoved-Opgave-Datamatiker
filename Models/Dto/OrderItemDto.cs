namespace Hoved_Opgave_Datamatiker.Models.Dto // Namespace for data transfer objects.
{
    /// <summary>
    /// DTO der repræsenterer en enkelt varelinje i en ordre.
    /// Indeholder produktinformation, mængde og enhedspris.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// ID for det bestilte produkt.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Antal af det bestilte produkt.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Det tilhørende produktobjekt med detaljer som navn, beskrivelse osv.
        /// </summary>
        public ProductDto Product { get; set; }

        /// <summary>
        /// Enhedspris på produktet i øre (cents).
        /// For eksempel, 2500 = 25,00 kr.
        /// </summary>
        public int UnitPrice { get; set; }
    }
}

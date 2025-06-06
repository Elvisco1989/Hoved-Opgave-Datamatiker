namespace Hoved_Opgave_Datamatiker.Models.Dto
{
    /// <summary>
    /// DTO that represents product details.
    /// Used to transfer product data between client and server.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the product.
        /// Uses decimal type for precise monetary values.
        /// </summary>
        public decimal Price { get; set; }

        // Commented out quantity property – can be used if product quantity info is needed in the DTO.
        // public int Quantity { get; set; }

        /// <summary>
        /// Description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Path or URL to an image representing the product.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Current stock level for the product.
        /// </summary>
        public int Stock { get; set; }
    }
}

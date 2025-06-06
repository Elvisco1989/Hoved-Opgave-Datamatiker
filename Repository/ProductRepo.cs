using Hoved_Opgave_Datamatiker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hoved_Opgave_Datamatiker.Repository
{
    /// <summary>
    /// In-memory repository der simulerer håndtering af produkter.
    /// Velegnet til test eller prototyping uden en rigtig database.
    /// </summary>
    public class ProductRepo : IProductRepo
    {
        // Starter fra 5, da de eksisterende produkter har Id'er 1-4
        private int nextId = 5;

        // Intern liste der simulerer en database tabel
        private List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Beer", Description = "NonAlco", Price = 100 },
            new Product { Id = 2, Name = "Beer", Description = "NonAlco", Price = 100 },
            new Product { Id = 3, Name = "Beer", Description = "NonAlco", Price = 100 },
            new Product { Id = 4, Name = "Beer", Description = "NonAlco", Price = 100 }
        };

        public ProductRepo() { }

        /// <summary>
        /// Tilføjer et nyt produkt og tildeler automatisk et unikt Id.
        /// </summary>
        /// <param name="product">Produktet der skal tilføjes.</param>
        /// <returns>Det tilføjede produkt med tildelt Id.</returns>
        public Product Addproduct(Product product)
        {
            product.Id = nextId++;
            products.Add(product);
            return product;
        }

        /// <summary>
        /// Henter et produkt baseret på Id.
        /// </summary>
        /// <param name="id">Produktets Id.</param>
        /// <returns>Produktet hvis fundet; ellers null.</returns>
        public Product? Getproduct(int id)
        {
            return products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Opdaterer et eksisterende produkt med nye værdier.
        /// </summary>
        /// <param name="product">Produktet med opdaterede oplysninger.</param>
        public void UpdateProduct(Product product)
        {
            var existingProduct = Getproduct(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Stock = product.Stock;
                existingProduct.ImagePath = product.ImagePath;
            }
        }

        /// <summary>
        /// Sletter et produkt baseret på Id.
        /// </summary>
        /// <param name="productId">Id på det produkt der skal slettes.</param>
        public void DeleteProduct(int productId)
        {
            var product = Getproduct(productId);
            if (product != null)
            {
                products.Remove(product);
            }
        }

        /// <summary>
        /// Henter alle produkter.
        /// </summary>
        /// <returns>En liste over alle produkter.</returns>
        public IEnumerable<Product> Getproducts()
        {
            return products;
        }
    }
}

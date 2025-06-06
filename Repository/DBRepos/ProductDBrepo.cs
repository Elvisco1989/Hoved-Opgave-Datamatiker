using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    /// <summary>
    /// Repositoryklasse til håndtering af Product-entiteter i databasen.
    /// Indeholder CRUD-operationer via Entity Framework Core.
    /// </summary>
    public class ProductDBrepo : IProductRepo
    {
        private readonly AppDBContext _Context;

        /// <summary>
        /// Initialiserer repository med databasekontekst.
        /// </summary>
        /// <param name="context">Instans af AppDBContext til databaseoperationer.</param>
        public ProductDBrepo(AppDBContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// Tilføjer et nyt produkt til databasen.
        /// </summary>
        /// <param name="product">Produktobjektet, der skal tilføjes.</param>
        /// <returns>Det tilføjede produkt.</returns>
        public Product Addproduct(Product product)
        {
            _Context.Products.Add(product);   // Tilføjer produkt til DbSet
            _Context.SaveChanges();            // Gemmer ændringer i databasen
            return product;
        }

        /// <summary>
        /// Sletter et produkt fra databasen.
        /// </summary>
        /// <param name="product">Produktobjektet, der skal slettes.</param>
        /// <returns>Det slettede produkt eller null, hvis det ikke blev fundet.</returns>
        public Product? Delete(Product product)
        {
            // Finder produktet i databasen via Id
            Product? product1 = Getproduct(product.Id);
            if (product1 != null)
            {
                _Context.Products.Remove(product1); // Fjerner produktet fra DbSet
                _Context.SaveChanges();             // Gemmer ændringer i databasen
            }

            return product1;
        }

        /// <summary>
        /// Henter et produkt baseret på produkt-ID.
        /// </summary>
        /// <param name="id">Produktets ID.</param>
        /// <returns>Produktobjektet eller null, hvis ikke fundet.</returns>
        public Product? Getproduct(int id)
        {
            return _Context.Products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Henter alle produkter fra databasen.
        /// </summary>
        /// <returns>En IEnumerable over alle produkter.</returns>
        public IEnumerable<Product> Getproducts()
        {
            return _Context.Products;
        }

        /// <summary>
        /// Opdaterer et eksisterende produkt i databasen.
        /// </summary>
        /// <param name="product">Produktobjektet med opdaterede oplysninger.</param>
        public void UpdateProduct(Product product)
        {
            var existingProduct = _Context.Products.Find(product.Id);
            if (existingProduct != null)
            {
                // Opdater felterne på det eksisterende produkt
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Stock = product.Stock;

                // Gem ændringer i databasen
                _Context.SaveChanges();
            }
        }
    }
}

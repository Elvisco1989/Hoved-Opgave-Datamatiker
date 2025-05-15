using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public class ProductRepo : IProductRepo
    {
        private int nextId = 0;
        private List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name="Beer", Description="NonAlco", Price=100},
            new Product { Id = 2, Name="Beer", Description="NonAlco", Price=100},
            new Product { Id = 3, Name="Beer", Description="NonAlco", Price=100},
            new Product { Id = 4, Name="Beer", Description="NonAlco", Price=100}
        };

        public ProductRepo() { }

        public Product Addproduct(Product product)
        {
            product.Id = nextId++;
            products.Add(product);
            return product;
        }
      

        public Product? Getproduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            return product;
        }

       
       

        public void UpdateProduct(Product product)
        {
            var existingProduct = Getproduct(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
              
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = Getproduct(productId);
            if (product != null)
            {
                products.Remove(product);
            }
        }

        public IEnumerable<Product> Getproducts()
        {
            return products;
        }
    }
}

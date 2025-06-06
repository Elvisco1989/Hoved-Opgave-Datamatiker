using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    public class ProductDBrepo : IProductRepo
    {
        private readonly AppDBContext _Context;

        public ProductDBrepo(AppDBContext context)
        {
            _Context = context;
        }

       

        public Product Addproduct(Product product)
        {
            _Context.Products.Add(product);
            _Context.SaveChanges();
            return product;
        }

        public Product? Delete(Product product)
        {
            // Assuming this means "Soft delete" by setting a flag
            Product? product1 = Getproduct(product.Id);
            if (product != null)
            {
               _Context?.Products.Remove(product1);
                _Context.SaveChanges();
            }
          
            return product1;
        }

        public Product? Getproduct(int id)
        {
            return _Context.Products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> Getproducts()
        {
            return _Context.Products;
                           
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _Context.Products.Find(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Stock = product.Stock;

                // Update other fields as needed

                _Context.SaveChanges();
            }
        }
    }

}

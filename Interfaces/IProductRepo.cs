using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public interface IProductRepo
    {
     
        Product Addproduct(Product product);
        Product? Getproduct(int id);
        public IEnumerable<Product> Getproducts();
        public void UpdateProduct(Product product);

        public Product? Delete(Product product);
    }
}
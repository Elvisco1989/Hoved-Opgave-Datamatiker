using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _Context;

        public ProductService(AppDBContext context)
        {
            _Context = context;
        }

        public List<LowStockProductDto> GetLowStockProducts(int threshold = 60)
        {
            return _Context.Products
                .Where(p => p.Stock < threshold)
                .Select(p => new LowStockProductDto
                {
                    Name = p.Name,
                    Stock = p.Stock
                })
                .ToList();
        }

    }
}

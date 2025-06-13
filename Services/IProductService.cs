using Hoved_Opgave_Datamatiker.Models.Dto;

namespace Hoved_Opgave_Datamatiker.Services
{
    public interface IProductService
    {
        List<LowStockProductDto> GetLowStockProducts(int threshold = 60);
    }
}
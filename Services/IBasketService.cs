using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    public interface IBasketService
    {
        void AddToBasket(int customerId, Product product, int quantity);
        void ClearBasket(int customerId);
        List<OrderItem> GetBasket(int customerId);
    }
}
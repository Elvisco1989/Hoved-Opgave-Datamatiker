using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    public class BasketService : IBasketService
    {
        private readonly Dictionary<int, List<OrderItem>> _customerBaskets = new();

        public void AddToBasket(int customerId, Product product, int quantity)
        {
            if (!_customerBaskets.ContainsKey(customerId))
                _customerBaskets[customerId] = new List<OrderItem>();

            _customerBaskets[customerId].Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                Product = product,
                UnitPrice = (int)product.Price
            });
        }

        public List<OrderItem> GetBasket(int customerId) =>
            _customerBaskets.ContainsKey(customerId) ? _customerBaskets[customerId] : new List<OrderItem>();

        public void ClearBasket(int customerId) =>
            _customerBaskets.Remove(customerId);
    }

}

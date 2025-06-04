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


        public void UpdateItemQuantity(int customerId, int productId, int quantity)
        {
            if (!_customerBaskets.ContainsKey(customerId))
                _customerBaskets[customerId] = new List<OrderItem>();

            var basket = _customerBaskets[customerId];
            var existingItem = basket.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
            }
            else
            {
                // You need access to the product to create a new OrderItem
                throw new InvalidOperationException("Product not in basket. Use AddToBasket instead.");
            }
        }

        public void RemoveItem(int customerId, int productId)
        {
            if (_customerBaskets.ContainsKey(customerId))
            {
                var basket = _customerBaskets[customerId];
                var existingItem = basket.FirstOrDefault(i => i.ProductId == productId);
                if (existingItem != null)
                {
                    basket.Remove(existingItem);
                }
                else
                {
                    throw new InvalidOperationException("Product not in basket.");
                }
            }
            else
            {
                throw new InvalidOperationException("Basket not found for customer.");
            }
        }







        public List<OrderItem> GetBasket(int customerId) =>
            _customerBaskets.ContainsKey(customerId) ? _customerBaskets[customerId] : new List<OrderItem>();

        public void ClearBasket(int customerId) =>
            _customerBaskets.Remove(customerId);
    }

}

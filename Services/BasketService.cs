using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Service til håndtering af kunders indkøbskurve (kurve).
    /// Bruger en intern ordbog til at gemme hver kundes kurv med deres ordrelinjer.
    /// </summary>
    public class BasketService : IBasketService
    {
        // Nøgle: KundeId, værdi: Liste af ordrelinjer (produkter i kurven)
        private readonly Dictionary<int, List<OrderItem>> _customerBaskets = new();

        /// <summary>
        /// Tilføjer et produkt med angivet antal til kundens kurv.
        /// </summary>
        public void AddToBasket(int customerId, Product product, int quantity)
        {
            if (!_customerBaskets.ContainsKey(customerId))
                _customerBaskets[customerId] = new List<OrderItem>();

            _customerBaskets[customerId].Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                Product = product,
                UnitPrice = (int)product.Price // Antaget at pris er i hele enheder (f.eks. cent)
            });
        }

        /// <summary>
        /// Opdaterer antallet af et produkt i kundens kurv.
        /// Hvis produktet ikke findes i kurven, kastes en undtagelse.
        /// </summary>
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
                throw new InvalidOperationException("Produktet findes ikke i kurven. Brug AddToBasket for at tilføje det.");
            }
        }

        /// <summary>
        /// Fjerner et produkt fra kundens kurv.
        /// Kaster undtagelse hvis produktet eller kurven ikke findes.
        /// </summary>
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
                    throw new InvalidOperationException("Produktet findes ikke i kurven.");
                }
            }
            else
            {
                throw new InvalidOperationException("Kurv findes ikke for kunden.");
            }
        }

        /// <summary>
        /// Henter hele kundens kurv (liste af ordrelinjer).
        /// Hvis kurv ikke findes, returneres en tom liste.
        /// </summary>
        public List<OrderItem> GetBasket(int customerId) =>
            _customerBaskets.ContainsKey(customerId) ? _customerBaskets[customerId] : new List<OrderItem>();

        /// <summary>
        /// Rydder kundens kurv helt.
        /// </summary>
        public void ClearBasket(int customerId) =>
            _customerBaskets.Remove(customerId);
    }
}

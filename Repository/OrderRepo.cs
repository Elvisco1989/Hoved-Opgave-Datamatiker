using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    /// <summary>
    /// En in-memory implementation af IOrderRepo til testformål eller midlertidig brug.
    /// Gemmer og håndterer ordrer i en lokal liste i stedet for en database.
    /// </summary>
    public class OrderRepo : IOrderRepo
    {
        private readonly List<Order> _orders = new(); // Lokal liste over ordrer
        private int _orderIdCounter = 1; // Simuleret auto-increment for ordre-ID'er

        /// <summary>
        /// Henter alle ordrer i systemet.
        /// </summary>
        /// <returns>En liste af alle ordrer.</returns>
        public IEnumerable<Order> GetOrders()
        {
            return _orders;
        }

        /// <summary>
        /// Henter en specifik ordre baseret på dens ID.
        /// </summary>
        /// <param name="id">Ordre-ID.</param>
        /// <returns>Ordren hvis den findes, ellers null.</returns>
        public Order GetOrderid(int id)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == id);
            return order;
        }

        /// <summary>
        /// Tilføjer et produkt som ordrelinje til en eksisterende ordre.
        /// </summary>
        /// <param name="orderId">ID på ordren, der skal opdateres.</param>
        /// <param name="product">Produktet der skal tilføjes.</param>
        /// <param name="quantity">Antal af produktet.</param>
        public void AddProductToOrder(int orderId, Product product, int quantity)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity
                };
                order.OrderItems.Add(orderItem);
            }
        }

        /// <summary>
        /// Tilføjer en ny ordre og tildeler automatisk et unikt ID.
        /// </summary>
        /// <param name="order">Ordren der skal tilføjes.</param>
        /// <returns>Den tilføjede ordre med tildelt ID.</returns>
        public Order AddOrder(Order order)
        {
            order.OrderId = _orderIdCounter++;
            _orders.Add(order);
            return order;
        }

        /// <summary>
        /// Opdaterer en eksisterende ordre med nye data.
        /// </summary>
        /// <param name="order">Den opdaterede ordre.</param>
        public void UpdateOrder(Order order)
        {
            var existingOrder = GetOrderid(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.customerId = order.customerId;
                existingOrder.OrderItems = order.OrderItems;
            }
        }

        /// <summary>
        /// Sletter en ordre baseret på dens ID.
        /// </summary>
        /// <param name="id">Ordre-ID der skal slettes.</param>
        /// <returns>Den slettede ordre hvis den fandtes, ellers null.</returns>
        public Order? DeleteOrder(int id)
        {
            Order? order = GetOrderid(id);
            if (order != null)
            {
                _orders.Remove(order);
            }
            return order;
        }
    }
}

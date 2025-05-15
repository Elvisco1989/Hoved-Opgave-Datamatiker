using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public class OrderRepo : IOrderRepo
    {

        private readonly List<Order> _orders = new()
        {

        };
        private int _orderIdCounter = 1;

        public IEnumerable<Order> GetOrders()
        {
            return _orders;
        }

        public Order GetOrderid(int id)
        {
            var order = _orders.FirstOrDefault(O => O.OrderId == id);
            return order;
        }
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
        //public Order GetOrder(int orderId)
        //{
        //    return _orders.FirstOrDefault(o => o.OrderId == orderId);
        //}
        public Order AddOrder(Order order)
        {
            // Assign a new OrderId to the order
            order.OrderId = _orderIdCounter++;
            _orders.Add(order);
            return order;
        }
        public void UpdateOrder(Order order)
        {
            var existingOrder = GetOrderid(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.customerId = order.customerId;
                existingOrder.OrderItems = order.OrderItems;
            }
        }

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

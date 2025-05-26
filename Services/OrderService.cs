using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _Context;

        public OrderService(AppDBContext context)
        {
            _Context = context;
        }

        // ✅ Create a new order for a customer
        public Order CreateOrder(int customerId)
        {
            var order = new Order
            {
                customerId = customerId,
                OrderDate = DateTime.UtcNow,
                PaymentStatus = "Pending",
                UnitPrice = 0, // Set default or calculated value
                TotalAmount = 0, // Set default or calculated value
                OrderItems = new List<OrderItem>()
            };

            _Context.Orders.Add(order);
            _Context.SaveChanges();

            return order;
        }

        // ✅ Add a product to an existing order
        public void AddProductToOrderDB(int orderId, Product product, int quantity)
        {
            var order = _Context.Orders
                                .Include(o => o.OrderItems)
                                .FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                var existingItem = order.OrderItems
                    .FirstOrDefault(oi => oi.ProductId == product.Id);

                if (existingItem != null)
                {
                    // If the item exists, just update the quantity
                    existingItem.Quantity += quantity;
                }
                else
                {
                    // Otherwise, add a new item
                    var orderItem = new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = product.Id,
                        Quantity = quantity,
                        UnitPrice = (int)product.Price
                    };

                    order.OrderItems.Add(orderItem);
                }

                UpdateTotalAmount(order);
                _Context.SaveChanges();
            }
        }


        // ✅ Helper to calculate order total
        private void UpdateTotalAmount(Order order)
        {
            order.TotalAmount = order.OrderItems
                                     .Sum(item => item.Quantity * item.UnitPrice);
        }

        // ✅ Get a specific order
        public Order? GetOrderById(int orderId)
        {
            return _Context.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(i => i.Product)
                           .FirstOrDefault(o => o.OrderId == orderId);
        }

        // ✅ Get all orders for a customer
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _Context.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(i => i.Product)
                           .Where(o => o.customerId == customerId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();
        }

        // ✅ Update payment status (e.g., after Stripe webhook)
        public void UpdatePaymentStatus(int orderId, string status)
        {
            var order = _Context.Orders.Find(orderId);
            if (order != null)
            {
                order.PaymentStatus = status;
                _Context.SaveChanges();
            }
        }

        // ✅ Delete order if needed (optional)
        public void DeleteOrder(int orderId)
        {
            var order = _Context.Orders
                                .Include(o => o.OrderItems)
                                .FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                _Context.OrderItems.RemoveRange(order.OrderItems);
                _Context.Orders.Remove(order);
                _Context.SaveChanges();
            }
        }

        public void AddProductToOrder(int orderId, Product product, int quantity)
        {
            throw new NotImplementedException();
        }


        //public IEnumerable<OrderItem> GetOrderItems(int orderId)
        //{
        //    return _Context.OrderItems
        //        .Include(oi => oi.Product)
        //        .Where(oi => oi.OrderId == orderId)
        //        .ToList();
        //}
    }

}

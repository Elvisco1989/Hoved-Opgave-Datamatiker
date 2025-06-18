using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Serviceklasse der håndterer forretningslogik for ordrer, 
    /// inklusiv oprettelse, opdatering, og hentning af ordrer.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _Context;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;

        /// <summary>
        /// Constructor med dependency injection af databasekontekst.
        /// </summary>
        public OrderService(AppDBContext context, IOrderRepo orderRepo, IProductRepo productRepo)
        {
            _Context = context;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public Order CreateOrder(int customerId)
        {
            var order = new Order
            {
                customerId = customerId,
                OrderDate = DateTime.UtcNow,
                PaymentStatus = "Pending",
                UnitPrice = 0,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            _Context.Orders.Add(order);
            _Context.SaveChanges();

            return order;
        }

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
                    existingItem.Quantity += quantity;
                }
                else
                {
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

        private void UpdateTotalAmount(Order order)
        {
            order.TotalAmount = order.OrderItems
                                     .Sum(item => item.Quantity * item.UnitPrice);
        }

        public Order? GetOrderById(int orderId)
        {
            return _Context.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(i => i.Product)
                           .FirstOrDefault(o => o.OrderId == orderId);
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _Context.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(i => i.Product)
                           .Where(o => o.customerId == customerId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();
        }

        public void UpdatePaymentStatus(int orderId, string status)
        {
            var order = _Context.Orders.Find(orderId);
            if (order != null)
            {
                order.PaymentStatus = status;
                _Context.SaveChanges();
            }
        }

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

        public List<TopSellingProductDto> GetTopSellingProducts()
        {
            var ordersWithItems = _Context.Orders
                .Include(o => o.OrderItems)
                .ToList();

            var allOrderItems = ordersWithItems
                .SelectMany(o => o.OrderItems)
                .ToList();

            var grouped = allOrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g =>
                {
                    var product = _Context.Products.Find(g.Key);
                    return new TopSellingProductDto
                    {
                        Name = product?.Name ?? "Unknown",
                        Price = product?.Price ?? 0,
                        QuantitySold = g.Sum(i => i.Quantity)
                    };
                })
                .OrderByDescending(p => p.QuantitySold)
                .ToList();

            return grouped;
        }

        public List<TopCustomerDto> GetTopCustomers()
        {
            var grouped = _Context.Orders
                .Where(o => o.Customer != null)
                .GroupBy(o => o.customerId)
                .Select(g => new TopCustomerDto
                {
                    Name = g.First().Customer.Name,
                    Address = g.First().Customer.Address,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(c => c.TotalRevenue)
                .ToList();

            return grouped;
        }

        public OrderMonthSummaryDto GetOrderSummary()
        {
            var now = DateTime.Now;

            var weekStart = now.AddDays(-(int)now.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            var allOrders = _Context.Orders.ToList();

            return new OrderMonthSummaryDto
            {
                TotalOrders = allOrders.Count,
                RevenueThisWeek = allOrders
                    .Where(o => o.OrderDate >= weekStart)
                    .Sum(o => o.TotalAmount),
                RevenueThisMonth = allOrders
                    .Where(o => o.OrderDate >= monthStart)
                    .Sum(o => o.TotalAmount),
                RevenueThisYear = allOrders
                    .Where(o => o.OrderDate >= yearStart)
                    .Sum(o => o.TotalAmount)
            };
        }
    }
}

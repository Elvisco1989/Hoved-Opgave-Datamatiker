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

<<<<<<< HEAD
        public OrderService(AppDBContext context, IOrderRepo orderRepo, IProductRepo productRepo)
=======
        /// <summary>
        /// Constructor med dependency injection af databasekontekst.
        /// </summary>
        /// <param name="context">AppDBContext til databaseadgang.</param>
        public OrderService(AppDBContext context)
>>>>>>> 96b5ca0fa3cc08fe13708d7fd00ad29cda7995b3
        {
            _Context = context;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        /// <summary>
        /// Opretter en ny ordre for en bestemt kunde med tom ordreliste og status "Pending".
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <returns>Den oprettede ordre.</returns>
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

        /// <summary>
        /// Tilføjer et produkt til en eksisterende ordre i databasen.
        /// Hvis produktet allerede findes i ordren, øges mængden.
        /// </summary>
        /// <param name="orderId">ID på ordren.</param>
        /// <param name="product">Produkt der skal tilføjes.</param>
        /// <param name="quantity">Antal af produktet.</param>
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

        /// <summary>
        /// Beregner den samlede pris for en ordre baseret på produkter og mængder.
        /// Intern hjælpermetode.
        /// </summary>
        /// <param name="order">Ordren der skal opdateres.</param>
        private void UpdateTotalAmount(Order order)
        {
            order.TotalAmount = order.OrderItems
                                     .Sum(item => item.Quantity * item.UnitPrice);
        }

        /// <summary>
        /// Henter en specifik ordre med produkter baseret på ordre-ID.
        /// </summary>
        /// <param name="orderId">Ordre-ID.</param>
        /// <returns>Ordren med ordrelinjer og tilknyttede produkter.</returns>
        public Order? GetOrderById(int orderId)
        {
            return _Context.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(i => i.Product)
                           .FirstOrDefault(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Henter alle ordrer for en bestemt kunde, sorteret efter dato (nyeste først).
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <returns>Liste af ordrer med produkter.</returns>
        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _Context.Orders
                           .Include(o => o.OrderItems)
                           .ThenInclude(i => i.Product)
                           .Where(o => o.customerId == customerId)
                           .OrderByDescending(o => o.OrderDate)
                           .ToList();
        }

        /// <summary>
        /// Opdaterer betalingsstatus for en ordre, f.eks. efter betaling via Stripe.
        /// </summary>
        /// <param name="orderId">Ordre-ID.</param>
        /// <param name="status">Ny betalingsstatus (f.eks. "Paid").</param>
        public void UpdatePaymentStatus(int orderId, string status)
        {
            var order = _Context.Orders.Find(orderId);
            if (order != null)
            {
                order.PaymentStatus = status;
                _Context.SaveChanges();
            }
        }

        /// <summary>
        /// Sletter en ordre og alle dens ordrelinjer fra databasen.
        /// </summary>
        /// <param name="orderId">Ordre-ID der skal slettes.</param>
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

        /// <summary>
        /// Ikke implementeret metode fra IOrderService interface. 
        /// Skal evt. bruges i test-scenarier.
        /// </summary>
        public void AddProductToOrder(int orderId, Product product, int quantity)
        {
            throw new NotImplementedException();
        }
<<<<<<< HEAD

        public List<TopSellingProductDto> GetTopSellingProducts()
        {
            // Step 1: Get all orders and include their OrderItems
            var ordersWithItems = _Context.Orders
                .Include(o => o.OrderItems)
                .ToList();

            // Step 2: Flatten all OrderItems into one list
            var allOrderItems = ordersWithItems
                .SelectMany(o => o.OrderItems)
                .ToList();

            // Step 3: Group by ProductId and compute total quantity sold
            var grouped = allOrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g =>
                {
                    var product = _Context.Products.Find(g.Key); // or use _productRepo.Getproduct(g.Key)
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
        // Services/OrderService.cs
        public OrderMonthSummaryDto GetOrderSummary()
        {
            var now = DateTime.Now;

            var weekStart = now.AddDays(-(int)now.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            var allOrders = _Context.Orders.ToList(); // Add filtering if needed
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








=======
>>>>>>> 96b5ca0fa3cc08fe13708d7fd00ad29cda7995b3
    }
}

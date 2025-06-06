using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hoved_Opgave_Datamatiker.Services;
using Stripe.Climate;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;
        private readonly ICustomerRepo _customerRepo;
        private IProductRepo _productRepo;
        private ICustomerService _customerService;
        private IBasketService _basketService;
        private readonly IOrderService _orderService;
        private IDeliveryDateService _deliveryDateService;

        /// <summary>
        /// Constructor til OrdersController med dependency injection af repositories og services.
        /// </summary>
        public OrdersController(IOrderRepo repo, ICustomerRepo customerRepo, IProductRepo productRepo, IBasketService basketService, IOrderService orderService, ICustomerService customerService, IDeliveryDateService deliveryDateService)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _basketService = basketService;
            _orderService = orderService;
            _customerService = customerService;
            _deliveryDateService = deliveryDateService;
        }

        /// <summary>
        /// Gennemfører en checkout-proces for en given kunde, opretter en ordre,
        /// reducerer lagerbeholdningen og returnerer ordredetaljer.
        /// </summary>
        /// <param name="customerId">ID for kunden der checker ud.</param>
        /// <returns>Et JSON-objekt med ordredetaljer eller en passende fejlbesked.</returns>
        [HttpPost("{customerId}/checkout")]
        public IActionResult Checkout(int customerId)
        {
            var customer = _customerRepo.GetCustomerById(customerId);
            if (customer == null) return NotFound("Customer not found");

            var basket = _basketService.GetBasket(customerId);
            if (basket.Count == 0) return BadRequest("Basket is empty");

            // Tjek lagerstatus
            foreach (var item in basket)
            {
                var product = _productRepo.Getproduct(item.Product.Id);
                if (product == null) return NotFound($"Product with ID {item.Product.Id} not found");

                if (product.Stock < item.Quantity)
                    return BadRequest($"Not enough stock for {product.Name}. Only {product.Stock} left.");
            }

            // Opret ordre
            var createdOrder = _orderService.CreateOrder(customerId);

            // Tilføj produkter til ordren og opdater lager
            foreach (var item in basket)
            {
                var product = _productRepo.Getproduct(item.Product.Id);
                product.Stock -= item.Quantity;
                _productRepo.UpdateProduct(product);
                _orderService.AddProductToOrderDB(createdOrder.OrderId, item.Product, item.Quantity);
            }

            _basketService.ClearBasket(customerId);

            // Find næste leveringsdato baseret på kundesegment
            DateTime? nextDelivery = null;
            if (Enum.TryParse<Segment>(customer.Segment, out var segmentEnum))
            {
                var deliveryDates = _deliveryDateService.GetDeliveryDatesForSegmentDB(segmentEnum, 10)
                                                         .Select(d => d.DeliveryDate)
                                                         .Where(d => d >= DateTime.Today)
                                                         .OrderBy(d => d)
                                                         .ToList();

                nextDelivery = deliveryDates.FirstOrDefault();
            }

            return Ok(new
            {
                orderId = createdOrder.OrderId,
                customerName = createdOrder.Customer.Name,
                customerAddress = createdOrder.Customer.Address,
                customerEmail = createdOrder.Customer.Email,
                customerSegment = customer.Segment,
                deliveryDates = customer.CustomerDeliveryDates.Select(d => d.DeliveryDate),
                paymentStatus = createdOrder.PaymentStatus,
                totalAmount = createdOrder.TotalAmount,
                nextDeliveryDate = nextDelivery ?? DateTime.UtcNow.AddDays(7),
                products = createdOrder.OrderItems.Select(item => new
                {
                    productName = item.Product.Name,
                    quantity = item.Quantity,
                    price = item.UnitPrice / 100m,
                    total = (item.UnitPrice * item.Quantity) / 100m
                }).ToList(),
                orderTotal = createdOrder.TotalAmount,
                message = "Order placed successfully"
            });
        }

        /// <summary>
        /// Henter en ordre baseret på ordre-ID og returnerer en samlet oversigt.
        /// </summary>
        /// <param name="id">Ordre-ID.</param>
        /// <returns>En oversigt over ordren med kundedata og produktdetaljer.</returns>
        [HttpGet("{id}")]
        public ActionResult<OrderSumDto> GetOrderById(int id)
        {
            var order = _repo.GetOrderid(id);
            if (order == null) return NotFound("Order not found");

            var customer = _customerRepo.GetCustomerById(order.customerId);
            if (customer == null) return NotFound("Customer not found");

            // Find næste leveringsdato
            var deliveryDates = _customerService?.GetDeliveryDatesForCustomer(customer.CustomerId);
            var nextDate = deliveryDates?
                .Where(d => d.DeliveryDate > DateTime.Today)
                .OrderBy(d => d.DeliveryDate)
                .FirstOrDefault()?.DeliveryDate ?? DateTime.Today.AddDays(7);

            // Byg produktliste
            var products = order.OrderItems.Select(oi =>
            {
                var product = _productRepo.Getproduct(oi.ProductId);
                if (product == null) return null;

                return new ProductOrderSummaryDto
                {
                    ProductName = product.Name,
                    Price = (int)product.Price,
                    Quantity = oi.Quantity
                };
            }).Where(p => p != null).ToList();

            // Returner samlet oversigt
            var summary = new OrderSumDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.Name,
                CustomerAddress = customer.Address,
                OrderId = order.OrderId,
                NextDeliveryDate = nextDate,
                Products = products
            };

            return Ok(summary);
        }
    }
}

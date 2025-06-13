using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Repository;
using Microsoft.AspNetCore.Mvc;
using Hoved_Opgave_Datamatiker.Services;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    /// <summary>
    /// Controller til håndtering af ordrer, herunder checkout og ordreopslag.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICustomerService _customerService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IDeliveryDateService _deliveryDateService;

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




        [HttpPost("{customerId}/checkout")]
        public IActionResult Checkout(int customerId)
        {
            var customer = _customerRepo.GetCustomerById(customerId);
            if (customer == null) return NotFound("Customer not found");

            var basket = _basketService.GetBasket(customerId);
            if (basket.Count == 0) return BadRequest("Basket is empty");

            // Check stock availability
            foreach (var item in basket)
            {
                var product = _productRepo.Getproduct(item.Product.Id);
                if (product == null) return NotFound($"Product with ID {item.Product.Id} not found");

                if (product.Stock < item.Quantity)
                    return BadRequest($"Not enough stock for {product.Name}. Only {product.Stock} left.");
            }

            // Opret ordre
            var createdOrder = _orderService.CreateOrder(customerId);

            // Add items to order & reduce stock
            foreach (var item in basket)
            {
                var product = _productRepo.Getproduct(item.Product.Id);
                product.Stock -= item.Quantity;
                _productRepo.UpdateProduct(product);
                _orderService.AddProductToOrderDB(createdOrder.OrderId, item.Product, item.Quantity);
            }

            _basketService.ClearBasket(customerId);

            // Get next delivery date based on customer segment
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


    





    [HttpGet("{id}")]
    public ActionResult<OrderSumDto> GetOrderById(int id)
    {
        var order = _repo.GetOrderid(id);
        if (order == null) return NotFound("Order not found");

            var customer = _customerRepo.GetCustomerById(order.customerId);
            if (customer == null) return NotFound("Customer not found");

        // Get future delivery dates and pick the next available
        var deliveryDates = _customerService?.GetDeliveryDatesForCustomer(customer.CustomerId);
        var nextDate = deliveryDates?
            .Where(d => d.DeliveryDate > DateTime.Today)
            .OrderBy(d => d.DeliveryDate)
            .FirstOrDefault()?.DeliveryDate ?? DateTime.Today.AddDays(7); // fallback if no dates

        // Build product list with total per item
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

        // Construct final order summary
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

        [HttpGet("products/top-sales")]
        public IActionResult GetTopSales()
        {
            var products = _orderService.GetTopSellingProducts();
            return Ok(products);
        }

        [HttpGet("customers/top")]
        public ActionResult<List<TopCustomerDto>> GetTopCustomers()
        {
            var topCustomers = _orderService.GetTopCustomers();
            return Ok(topCustomers);
        }

        [HttpGet("summary")]
        public ActionResult<OrderMonthSummaryDto> GetOrderSummary()
        {
            return Ok(_orderService.GetOrderSummary());
        }







    }
}

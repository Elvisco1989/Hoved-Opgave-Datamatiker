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

        public OrdersController(IOrderRepo repo, ICustomerRepo customerRepo, IProductRepo productRepo, IBasketService basketService, IOrderService orderService, ICustomerService customerService)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _basketService = basketService;
            _orderService = orderService;
            _customerService = customerService;
        }



        [HttpPost("{customerId}/checkout")]
        public IActionResult Checkout(int customerId)
        {
            var customer = _customerRepo.GetCustomerById(customerId);
            if (customer == null) return NotFound("Customer not found");

            var basket = _basketService.GetBasket(customerId);
            if (basket.Count == 0) return BadRequest("Basket is empty");

            // Create a new order in the database
            var createdOrder = _orderService.CreateOrder(customerId);

            // Add each basket item to the order
            foreach (var item in basket)
            {
                _orderService.AddProductToOrderDB(createdOrder.OrderId, item.Product, item.Quantity);
            }

            // Optionally recalculate total (already done in AddProductToOrderDB)

            _basketService.ClearBasket(customerId);

            return Ok(new
            {
                createdOrder.OrderId,
                createdOrder.TotalAmount,
                Message = "Order placed successfully"
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



    }
}

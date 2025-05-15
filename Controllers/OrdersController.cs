using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hoved_Opgave_Datamatiker.Services;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;
        private readonly ICustomerRepo _customerRepo;
        private IProductRepo _productRepo;
        private ICustomerService customerService;

        public OrdersController(IOrderRepo repo, ICustomerRepo customerRepo, IProductRepo productRepo)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
        }

        [HttpPost]
        public ActionResult<OrderDto> PlaceOrder([FromBody] CreateOrderDto dto)
        {
            var customer = _customerRepo.GetCustomerById(dto.CustomerId);
            if (customer == null)
                return NotFound("Customer not found.");

            var order = new Order
            {
                customerId = dto.CustomerId,
                OrderItems = dto.OrderItems.Select(oi =>
                {
                    var product = _productRepo.Getproduct(oi.ProductId);
                    return new OrderItem
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Product = product
                    };
                }).ToList()
            };

            var createdOrder = _repo.AddOrder(order);
            customer.Orders.Add(createdOrder);

            var orderDto = new OrderDto
            {
                OrderId = createdOrder.OrderId,
                CustomerId = createdOrder.customerId,
                OrderItems = createdOrder.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Product = null // or omit this field in DTO
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, orderDto);


        }
        [HttpGet("{id}")]
        public ActionResult<OrderSummaryDto> GetOrderById(int id)
        {
            var order = _repo.GetOrderid(id);
            if (order == null) return NotFound();

            var customer = _customerRepo.GetCustomerById(order.customerId);
            if (customer == null) return NotFound();

            var deliveryDates = customerService.GetDeliveryDatesForCustomer(customer.CustomerId);
            var nextDate = deliveryDates.OrderBy(d => d.DeliveryDate).FirstOrDefault()?.DeliveryDate;

            var products = order.OrderItems.Select(oi =>
            {
                var product = _productRepo.Getproduct(oi.ProductId);
                return new ProductOrderSummaryDto
                {
                    ProductName = product.Name,
                    Price = (int)product.Price,
                    Quantity = oi.Quantity
                };
            }).ToList();

            var summary = new OrderSumDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.Name,
                CustomerAddress = customer.Address,
                NextDeliveryDate = nextDate ?? DateTime.MinValue,
                OrderId = order.OrderId,
                Products = products
            };

            return Ok(summary);
        }



    }
}

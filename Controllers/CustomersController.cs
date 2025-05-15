using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IDeliveryDateRepo _deliveryDateRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICustomerService _customerService;
        private readonly IDeliveryDateService _deliveryDateService;

        public CustomersController(ICustomerRepo customerRepo, IDeliveryDateRepo deliveryDateRepo, IProductRepo productRepo, ICustomerService customerService, IDeliveryDateService deliveryDateService)
        {
            _customerRepo = customerRepo;
            _deliveryDateRepo = deliveryDateRepo;
            _productRepo = productRepo;
            _customerService = customerService;
            _deliveryDateService = deliveryDateService;
        }

        // GET: api/customers
        [HttpGet]
        public ActionResult<List<CustomerDto>> GetAllCustomers()
        {
            var customers = _customerRepo.GetAllCustomers();

            if (customers == null || !customers.Any())
                return NotFound("No customers found.");

            var customerDtos = customers.Select(c =>
            {
                var deliveryDates = _customerService.GetDeliveryDatesForCustomer(c.CustomerId);
                var deliveryDateList = deliveryDates.Select(dd => dd.DeliveryDate).ToList();

                return MapToCustomerDto(c, deliveryDateList);
            }).ToList();

            return Ok(customerDtos);
        }


        // GET: api/customers/5
        [HttpGet("{id}")]
        public ActionResult<CustomerDto> GetCustomerById(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            var deliveryDates = _customerService.GetDeliveryDatesForCustomer(id);
            var dateTimes = deliveryDates.Select(dd => dd.DeliveryDate).ToList();

            return Ok(MapToCustomerDto(customer, dateTimes));
        }


        // POST: api/customers
        [HttpPost]
        [HttpPost]
        public ActionResult<CustomerDto> AddCustomer([FromBody] CreateCustomer createCustomer)
        {
            var customer = new Customer
            {
                Name = createCustomer.Name,
                Address = createCustomer.Address,
                Segment = createCustomer.Segment,
                PhoneNumber = createCustomer.PhoneNumber,
                Email = createCustomer.Email
            };

            // Add the customer to the repository
            var addedCustomer = _customerRepo.AddCustomer(customer);

            // Assign delivery dates based on the customer's segment
            if (Enum.TryParse<Segment>(customer.Segment, out var segmentEnum))
            {
                var matchingDates = _deliveryDateService.GetDeliveryDatesForSegment(segmentEnum, 10);

                // Assign delivery dates to the customer
                _customerService.AssignDeliveryDates(addedCustomer, matchingDates);

                // Get the DateTime values directly from the `matchingDates`
                var deliveryDateList = matchingDates.Select(dd => dd.DeliveryDate).ToList();

                // Return the mapped customer with delivery dates
                return CreatedAtAction(nameof(GetCustomerById), new { id = addedCustomer.CustomerId }, MapToCustomerDto(addedCustomer, deliveryDateList));
            }

            return BadRequest("Invalid segment value.");
        }

        // Helper to map Customer to DTO
        private CustomerDto MapToCustomerDto(Customer customer, List<DateTime> deliveryDates)
        {
            foreach (var order in customer.Orders)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Product = _productRepo.Getproduct(item.ProductId);
                }
            }

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Address = customer.Address,
                Segment = customer.Segment,
                DeliveryDates = deliveryDates,
                Orders = customer.Orders.Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    CustomerId = order.customerId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Product = oi.Product != null ? new ProductDto
                        {
                            ProductId = oi.Product.Id,
                            Name = oi.Product.Name,
                            Price = oi.Product.Price,
                            Description = oi.Product.Description
                        } : null
                    }).ToList()
                }).ToList()
            };
        }

    }
}

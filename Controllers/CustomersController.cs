using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    /// <summary>
    /// Controller til håndtering af kunder og deres levering.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IDeliveryDateRepo _deliveryDateRepo;
        private readonly IProductRepo _productRepo;

        private readonly ICustomerService _customerService;
        private readonly IDeliveryDateService _deliveryDateService;
        private readonly AppDBContext context;

        /// <summary>
        /// Initialiserer en ny instans af <see cref="CustomersController"/>.
        /// </summary>
        public CustomersController(
            ICustomerRepo customerRepo,
            IDeliveryDateRepo deliveryDateRepo,
            IProductRepo productRepo,
            ICustomerService customerService,
            IDeliveryDateService deliveryDateService,
            AppDBContext context)
        {
            _customerRepo = customerRepo;
            _deliveryDateRepo = deliveryDateRepo;
            _productRepo = productRepo;
            _customerService = customerService;
            _deliveryDateService = deliveryDateService;
            this.context = context;
        }

        /// <summary>
        /// Henter alle kunder med tilknyttede leveringsdatoer og ordrer.
        /// </summary>
        /// <returns>En liste af <see cref="CustomerDto"/> objekter.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerDto>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<List<CustomerDto>> GetAllCustomers()
        {
            var customers = _customerRepo.GetAllCustomers().ToList();

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

        /// <summary>
        /// Henter en kunde med tilhørende leveringsdatoer og ordrer baseret på ID.
        /// </summary>
        /// <param name="id">Kundens ID.</param>
        /// <returns>En <see cref="CustomerDto"/> hvis fundet, ellers 404.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(404)]
        public ActionResult<CustomerDto> GetCustomerById(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            var deliveryDates = _customerService.GetDeliveryDatesForCustomer(id);
            var dateTimes = deliveryDates.Select(dd => dd.DeliveryDate).ToList();

            return Ok(MapToCustomerDto(customer, dateTimes));
        }

        /// <summary>
        /// Tilføjer en ny kunde og tildeler leveringsdatoer baseret på segment.
        /// </summary>
        /// <param name="createCustomer">Data til oprettelse af kunden.</param>
        /// <returns>Den oprettede kunde som <see cref="CustomerDto"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 201)]
        [ProducesResponseType(400)]
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

            var addedCustomer = _customerRepo.AddCustomer(customer);

            if (Enum.TryParse<Segment>(customer.Segment, out var segmentEnum))
            {
                var matchingDates = _deliveryDateService.GetDeliveryDatesForSegmentDB(segmentEnum, 10);
                _customerService.AssignDeliveryDates(addedCustomer, matchingDates);
                var deliveryDateList = matchingDates.Select(dd => dd.DeliveryDate).ToList();

                return CreatedAtAction(nameof(GetCustomerById), new { id = addedCustomer.CustomerId }, MapToCustomerDto(addedCustomer, deliveryDateList));
            }

            return BadRequest("Invalid segment value.");
        }

        /// <summary>
        /// Sletter en kunde baseret på ID.
        /// </summary>
        /// <param name="id">Kundens ID.</param>
        /// <returns>Statuskode 204 ved succes, 404 hvis ikke fundet.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            _customerRepo.DeleteCustomer(id);
            return NoContent(); // 204
        }

        /// <summary>
        /// Mapper en <see cref="Customer"/> til en <see cref="CustomerDto"/>, inkl. produkter i ordrer.
        /// </summary>
        /// <param name="customer">Kundeobjektet.</param>
        /// <param name="deliveryDates">Liste af leveringsdatoer.</param>
        /// <returns>Et <see cref="CustomerDto"/> objekt.</returns>
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

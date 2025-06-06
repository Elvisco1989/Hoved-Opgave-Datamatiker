using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Pay;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly AppDBContext _context;

        public PaymentsController(PaymentService paymentService, AppDBContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpPost("{orderId}")]
        public IActionResult CreatePaymentIntent(int orderId)
        {
            var order = _context.Orders
                                .Include(o => o.OrderItems)
                                .ThenInclude(i => i.Product)
                                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null) return NotFound("Order not found");

            var intent = _paymentService.CreateAndAttachPaymentIntent(order);

            return Ok(new
            {
                clientSecret = intent.ClientSecret,
                paymentIntentId = intent.Id
            });
        }
    }
}

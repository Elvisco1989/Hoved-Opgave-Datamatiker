using Hoved_Opgave_Datamatiker.Pay;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hoved_Opgave_Datamatiker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentsController(PaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost("create-payment-intent")]
        public IActionResult CreatePaymentIntent([FromBody] int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                return NotFound("Order not found");

            var intent = _paymentService.CreateAndAttachPaymentInten(order.TotalAmount);

            return Ok(new
            {
                clientSecret = intent.ClientSecret
            });
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Hoved_Opgave_Datamatiker.Pay;
using Hoved_Opgave_Datamatiker.Repository;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly IOrderRepo _orderRepo;

    public PaymentsController(PaymentService paymentService, IOrderRepo orderRepo)
    {
        _paymentService = paymentService;
        _orderRepo = orderRepo;
    }

    [HttpPost("{orderId}/create-payment-intent")]
    public IActionResult CreatePaymentIntent(int orderId)
    {
        var order = _orderRepo.GetOrderid(orderId);
        if (order == null) return NotFound("Order not found");

        var paymentIntent = _paymentService.CreateAndAttachPaymentIntent(order);

        return Ok(new { clientSecret = paymentIntent.ClientSecret });
    }
}


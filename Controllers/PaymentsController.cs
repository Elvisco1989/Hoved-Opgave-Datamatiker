// Importerer nødvendige namespaces.
using Hoved_Opgave_Datamatiker.DBContext; // Indeholder AppDBContext-klassen (Entity Framework databasekontekst).
using Hoved_Opgave_Datamatiker.Pay; // Indeholder betalingsrelaterede klasser og logik, fx PaymentIntent.
using Hoved_Opgave_Datamatiker.Services; // Indeholder serviceklasser, som fx PaymentService.
using Microsoft.AspNetCore.Http; // Indeholder typer relateret til HTTP-kontext.
using Microsoft.AspNetCore.Mvc; // Understøtter ASP.NET Core MVC og API-controllere.
using Microsoft.EntityFrameworkCore; // Til Entity Framework-funktioner, fx Include og ThenInclude.

namespace Hoved_Opgave_Datamatiker.Controllers // Definerer controllerens namespace.
{
    [Route("api/[controller]")] // API-ruten bliver fx /api/payments.
    [ApiController] // Angiver at dette er en API-controller, som understøtter automatiske valideringer m.m.
    public class PaymentsController : ControllerBase // Arver fra ControllerBase (ingen views, kun API).
    {
        private readonly PaymentService _paymentService; // Service til at håndtere betalinger.
        private readonly AppDBContext _context; // Databasekontekst til datatilgang via Entity Framework.

        /// <summary>
        /// Constructor der modtager afhængigheder via dependency injection.
        /// </summary>
        /// <param name="paymentService">Service til betalingshåndtering.</param>
        /// <param name="context">Databasekontekst.</param>
        public PaymentsController(PaymentService paymentService, AppDBContext context)
        {
            _paymentService = paymentService; // Initialiserer _paymentService.
            _context = context; // Initialiserer databasekonteksten.
        }

        /// <summary>
        /// Opretter en betalingsintention for en given ordre.
        /// </summary>
        /// <param name="orderId">ID for den ordre, der skal betales for.</param>
        /// <returns>Client secret og betalingsintent-id hvis succesfuld.</returns>
        [HttpPost("{orderId}")] // POST-endpoint: fx POST /api/payments/5
        public IActionResult CreatePaymentIntent(int orderId)
        {
            // Henter ordren inklusiv relaterede OrderItems og deres produkter.
            var order = _context.Orders
                                .Include(o => o.OrderItems) // Medtag relaterede OrderItems
                                .ThenInclude(i => i.Product) // Medtag produktinfo til hvert item
                                .FirstOrDefault(o => o.OrderId == orderId); // Finder ordren ud fra ID

            if (order == null) return NotFound("Order not found"); // Returnerer 404 hvis ordren ikke findes.

            // Opretter en betalingsintent og knytter den til ordren.
            var intent = _paymentService.CreateAndAttachPaymentIntent(order);

            // Returnerer betalingsoplysninger til frontend.
            return Ok(new
            {
                clientSecret = intent.ClientSecret, // Bruges til at gennemføre betalingen på klienten.
                paymentIntentId = intent.Id // Unikt ID for betalingsintentionen.
            });
        }
    }
}

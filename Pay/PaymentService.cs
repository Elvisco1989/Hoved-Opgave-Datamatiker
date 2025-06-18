using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hoved_Opgave_Datamatiker.Pay
{
    /// <summary>
    /// Serviceklasse til håndtering af betalinger via Stripe.
    /// Ansvarlig for at oprette og knytte en PaymentIntent til en ordre.
    /// </summary>
    public class PaymentService
    {
        private readonly AppDBContext _context;
        private readonly IStripeClient _stripeClient;

        /// <summary>
        /// Constructor med dependency injection af databasekontekst og StripeClient.
        /// </summary>
        /// <param name="context">AppDBContext til databaseadgang.</param>
        /// <param name="stripeClient">IStripeClient til Stripe API adgang.</param>
        public PaymentService(AppDBContext context, IStripeClient stripeClient)
        {
            _context = context;
            _stripeClient = stripeClient;
        }

        /// <summary>
        /// Opretter en Stripe PaymentIntent og knytter den til en eksisterende ordre.
        /// Gemmer PaymentIntent-ID og sætter betalingsstatus til "Pending".
        /// </summary>
        /// <param name="order">Ordren der skal betales for.</param>
        /// <returns>Den oprettede Stripe PaymentIntent.</returns>
        public PaymentIntent CreateAndAttachPaymentIntent(Order order)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100), // Stripe bruger cents
                Currency = "dkk",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                }
            };

            var service = new PaymentIntentService(_stripeClient);
            var intent = service.Create(options);

            // Gem Stripe PaymentIntent ID og status i databasen
            order.StripePaymentIntentId = intent.Id;
            order.PaymentStatus = "Pending";
            _context.Orders.Update(order);
            _context.SaveChanges();

            return intent;
        }
    }
}
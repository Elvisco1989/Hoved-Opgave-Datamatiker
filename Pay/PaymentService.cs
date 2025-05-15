using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hoved_Opgave_Datamatiker.Pay
{
    public class PaymentService
    {
        private readonly AppDBContext _context;

        public PaymentService(AppDBContext context)
        {
            _context = context;
        }

        public PaymentIntent CreateAndAttachPaymentIntent(Order order)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                }
            };

            var service = new PaymentIntentService();
            var intent = service.Create(options);

            // Save Stripe PaymentIntent ID and status to database
            order.StripePaymentIntentId = intent.Id;
            order.PaymentStatus = "Pending";
            _context.Orders.Update(order); // Good practice
            _context.SaveChanges();

            return intent;
        }
    }

}

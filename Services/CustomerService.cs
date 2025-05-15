using Hoved_Opgave_Datamatiker.Interfaces;
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    public class CustomerService : ICustomerService
    {
        private List<CustomerDeliveryDates> _links = new List<CustomerDeliveryDates>();
        public void AssignDeliveryDates(Customer customer, List<DeliveryDates> deliveryDates)
        {
            foreach (var date in deliveryDates)
            {
                _links.Add(new CustomerDeliveryDates
                {
                    CustomerId = customer.CustomerId,
                    DeliveryDateId = date.DeliveryDateId,
                    Customer = customer,
                    DeliveryDate = date
                });
            }
        }

        public List<CustomerDeliveryDateDto> GetDeliveryDatesForCustomer(int customerId)
        {
            return _links
                .Where(l => l.CustomerId == customerId)
                .Select(l => new CustomerDeliveryDateDto
                {
                    DeliveryDate = l.DeliveryDate.DeliveryDate,
                })
                .ToList();
        }


    }
}

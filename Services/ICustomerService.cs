using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    public interface ICustomerService
    {
        void AssignDeliveryDates(Customer customer, List<DeliveryDates> deliveryDates);
        List<CustomerDeliveryDateDto> GetDeliveryDatesForCustomer(int customerId);
    }
}
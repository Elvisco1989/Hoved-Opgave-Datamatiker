using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public interface IDeliveryDateRepo
    {
        DeliveryDates AddDeliveryDate(DeliveryDates deliveryDate);
        void DeleteDeliveryDate(int id);
        List<DeliveryDates> GetAllDeliveryDates();
        DeliveryDates GetDeliveryDateById(int id);
       
        void UpdateDeliveryDate(DeliveryDates deliveryDate);
    }
}
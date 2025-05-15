using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Services
{
    public interface IOrderService
    {
        Order CreateOrder(int customerId);
        void AddProductToOrderDB(int orderId, Product product, int quantity);
        Order? GetOrderById(int orderId);
        List<Order> GetOrdersByCustomer(int customerId);
        void UpdatePaymentStatus(int orderId, string status);
        void DeleteOrder(int orderId);
        void AddProductToOrder(int orderId, Product product, int quantity);
       
    }
}
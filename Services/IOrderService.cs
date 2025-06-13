using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;

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

        List<TopSellingProductDto> GetTopSellingProducts();

        List<TopCustomerDto> GetTopCustomers();
        OrderMonthSummaryDto GetOrderSummary();




        //IEnumerable<OrderItem> GetOrderItems(int orderId);


    }
}
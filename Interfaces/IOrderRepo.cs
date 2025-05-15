using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository
{
    public interface IOrderRepo
    {
        Order AddOrder(Order order);
       
        //Order GetOrder(int orderId);
        Order GetOrderid(int id);
        public IEnumerable<Order> GetOrders();
        void UpdateOrder(Order order);
        public Order? DeleteOrder(int id);
        
        }
}
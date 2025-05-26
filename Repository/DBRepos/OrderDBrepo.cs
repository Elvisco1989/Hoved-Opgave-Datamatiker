using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    public class OrderDBrepo : IOrderRepo
    {
        private readonly AppDBContext _Context;

        public OrderDBrepo(AppDBContext context)
        {
            _Context = context;
        }

        public Order AddOrder(Order order)
        {
            _Context.Orders.Add(order);
            return order;
        }

       

        public Order? DeleteOrder(int id)
        {
            var order = GetOrderid(id);
            if (order != null) 
            {
                _Context.Remove(order);
            }
            return order;


        }

      

        public Order GetOrderid(int id)
        {
            var order = _Context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null) 
            {
                return null;
            }
            return order;
        }



        public IEnumerable<Order> GetOrders()
        {
          return _Context.Orders;
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = GetOrderid(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.customerId = order.customerId;
                existingOrder.OrderItems = order.OrderItems;
            }
        }
    }
}

using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Models;

namespace Hoved_Opgave_Datamatiker.Repository.DBRepos
{
    /// <summary>
    /// Implementation af IOrderRepo som håndterer CRUD-operationer for ordrer
    /// via Entity Framework og AppDBContext.
    /// </summary>
    public class OrderDBrepo : IOrderRepo
    {
        private readonly AppDBContext _Context;

        /// <summary>
        /// Constructor der modtager en instans af AppDBContext via dependency injection.
        /// </summary>
        /// <param name="context">Databasekonteksten.</param>
        public OrderDBrepo(AppDBContext context)
        {
            _Context = context;
        }

        /// <summary>
        /// Tilføjer en ny ordre til databasen (føjes til DbSet, men gemmes ikke automatisk).
        /// </summary>
        /// <param name="order">Ordren der skal tilføjes.</param>
        /// <returns>Den tilføjede ordre.</returns>
        public Order AddOrder(Order order)
        {
            _Context.Orders.Add(order);
            return order;
        }

        /// <summary>
        /// Sletter en ordre baseret på ordre-ID.
        /// </summary>
        /// <param name="id">ID på ordren der skal slettes.</param>
        /// <returns>Den slettede ordre hvis den fandtes; ellers null.</returns>
        public Order? DeleteOrder(int id)
        {
            var order = GetOrderid(id);
            if (order != null)
            {
                _Context.Remove(order);
            }
            return order;
        }

        /// <summary>
        /// Henter en ordre baseret på dens ID.
        /// </summary>
        /// <param name="id">Ordre-ID.</param>
        /// <returns>Ordren hvis den findes; ellers null.</returns>
        public Order GetOrderid(int id)
        {
            var order = _Context.Orders.FirstOrDefault(o => o.OrderId == id);
            return order;
        }

        /// <summary>
        /// Returnerer alle ordrer i databasen.
        /// </summary>
        /// <returns>En IEnumerable af alle ordrer.</returns>
        public IEnumerable<Order> GetOrders()
        {
            return _Context.Orders;
        }

        /// <summary>
        /// Opdaterer en eksisterende ordre hvis den findes.
        /// Ændrer kunde-ID og ordrelinjer.
        /// </summary>
        /// <param name="order">Den opdaterede ordre.</param>
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

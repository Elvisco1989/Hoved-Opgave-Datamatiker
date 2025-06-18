using Hoved_Opgave_Datamatiker.Models;
using Hoved_Opgave_Datamatiker.Models.Dto;

namespace Hoved_Opgave_Datamatiker.Services
{
    /// <summary>
    /// Interface for service, der håndterer forretningslogik relateret til ordrer.
    /// Indeholder metoder til oprettelse, opdatering, sletning og forespørgsler på ordrer.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Opretter en ny ordre for en kunde.
        /// </summary>
        /// <param name="customerId">ID på kunden, der opretter ordren.</param>
        /// <returns>Den oprettede ordre.</returns>
        Order CreateOrder(int customerId);

        /// <summary>
        /// Tilføjer et produkt til en ordre og gemmer det i databasen.
        /// </summary>
        /// <param name="orderId">Ordre-ID.</param>
        /// <param name="product">Produktet der skal tilføjes.</param>
        /// <param name="quantity">Antal af produktet.</param>
        void AddProductToOrderDB(int orderId, Product product, int quantity);

        /// <summary>
        /// Henter en specifik ordre med ordrelinjer og tilhørende produkter.
        /// </summary>
        /// <param name="orderId">ID på ordren.</param>
        /// <returns>Ordren, hvis den findes; ellers null.</returns>
        Order? GetOrderById(int orderId);

        /// <summary>
        /// Henter alle ordrer for en given kunde.
        /// </summary>
        /// <param name="customerId">ID på kunden.</param>
        /// <returns>Liste over ordrer tilhørende kunden.</returns>
        List<Order> GetOrdersByCustomer(int customerId);

        /// <summary>
        /// Opdaterer betalingsstatus for en specifik ordre.
        /// </summary>
        /// <param name="orderId">ID på ordren.</param>
        /// <param name="status">Den nye betalingsstatus (f.eks. "Paid", "Failed").</param>
        void UpdatePaymentStatus(int orderId, string status);

        /// <summary>
        /// Sletter en ordre samt dens tilknyttede ordrelinjer.
        /// </summary>
        /// <param name="orderId">ID på ordren der skal slettes.</param>
        void DeleteOrder(int orderId);

        /// <summary>
        /// (Valgfri/Not Implemented) Tilføjer et produkt til en ordre uden at gemme direkte i databasen.
        /// Kan bruges til test eller alternativ logik.
        /// </summary>
        /// <param name="orderId">Ordre-ID.</param>
        /// <param name="product">Produktet.</param>
        /// <param name="quantity">Antal.</param>
        void AddProductToOrder(int orderId, Product product, int quantity);

        /// <summary>
        /// Returnerer en liste over de mest solgte produkter.
        /// </summary>
        /// <returns>Liste af TopSellingProductDto.</returns>
        List<TopSellingProductDto> GetTopSellingProducts();

        /// <summary>
        /// Returnerer en liste over de kunder, der har genereret mest omsætning.
        /// </summary>
        /// <returns>Liste af TopCustomerDto.</returns>
        List<TopCustomerDto> GetTopCustomers();

        /// <summary>
        /// Returnerer statistik for ordrer og omsætning fordelt på uge, måned og år.
        /// </summary>
        /// <returns>Et OrderMonthSummaryDto-objekt med summerede værdier.</returns>
        OrderMonthSummaryDto GetOrderSummary();

        // Eksempel på fremtidig metode:
        // IEnumerable<OrderItem> GetOrderItems(int orderId);
    }
}

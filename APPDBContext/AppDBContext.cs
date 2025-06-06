using Hoved_Opgave_Datamatiker.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoved_Opgave_Datamatiker.DBContext
{
    /// <summary>
    /// Entity Framework Core database context for the application.
    /// Provides DbSet properties for Customer, Product, Order, DeliveryDates,
    /// CustomerDeliveryDates, and OrderItem entities.
    /// Configures composite keys and relationships between entities.
    /// </summary>
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryDates> DeliveryDates { get; set; }

        public DbSet<CustomerDeliveryDates> CustomerDeliveryDates { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the composite key for CustomerDeliveryDates
            modelBuilder.Entity<CustomerDeliveryDates>()
                .HasKey(cd => new { cd.CustomerId, cd.DeliveryDateId });

            // Configure the relationship between Customer and CustomerDeliveryDates
            modelBuilder.Entity<CustomerDeliveryDates>()
                .HasOne(cd => cd.Customer)
                .WithMany(c => c.CustomerDeliveryDates)
                .HasForeignKey(cd => cd.CustomerId);

            // Configure the relationship between DeliveryDates and CustomerDeliveryDates
            modelBuilder.Entity<CustomerDeliveryDates>()
                .HasOne(cd => cd.DeliveryDate)
                .WithMany(d => d.CustomerDeliveryDates)
                .HasForeignKey(cd => cd.DeliveryDateId);

            // Define composite key for OrderItem (OrderId, ProductId)
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ProductId });

            // Define relationships for OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}

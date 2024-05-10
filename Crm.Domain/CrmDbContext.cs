using Crm.Domain.Configurations;
using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Crm.Domain
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options)
            : base(options)
        {
        }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Checks> Checks { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Deliveries> Deliveries { get; set; }
        public DbSet<DishCategories> DishCategories { get; set; }
        public DbSet<DishComponents> DishComponents { get; set; }
        public DbSet<Dishes> Dishes { get; set; }
        public DbSet<EmployeeRoles> EmployeeRoles { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Loyalities> Loyalities { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<ProductCategories> ProductCategories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<Storages> Storages { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountsConfiguration());
            modelBuilder.ApplyConfiguration(new ChecksConfiguration());
            modelBuilder.ApplyConfiguration(new ClientsConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveriesConfiguration());
            modelBuilder.ApplyConfiguration(new DishCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new DishComponentsConfiguration());
            modelBuilder.ApplyConfiguration(new DishesConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeRolesConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeesConfiguration());
            modelBuilder.ApplyConfiguration(new InvoicesConfiguration());
            modelBuilder.ApplyConfiguration(new LoyalitiesConfiguration());
            modelBuilder.ApplyConfiguration(new OrdersConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new ProvidersConfiguration());
            modelBuilder.ApplyConfiguration(new StoragesConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusConfiguration());
            modelBuilder.UseSerialColumns();
            base.OnModelCreating(modelBuilder);
        }
    }
}

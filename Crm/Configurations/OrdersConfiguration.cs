using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .HasOne(c => c.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId);

        builder
            .HasOne(o => o.Payment)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PaymentId);

        builder
            .HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId);

        builder
            .HasMany(o => o.Checks)
            .WithOne(ch => ch.Order)
            .HasForeignKey(ch => ch.OrderId);

        builder
            .HasOne(o => o.OrderStatus)
            .WithMany(os => os.Orders)
            .HasForeignKey(o => o.OrderStatusId);
    }
    
}
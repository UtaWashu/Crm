using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.HasKey(os => os.Id);

        builder
            .HasMany(os => os.Orders)
            .WithOne(o => o.OrderStatus)
            .HasForeignKey(o => o.OrderStatusId);
    }
}
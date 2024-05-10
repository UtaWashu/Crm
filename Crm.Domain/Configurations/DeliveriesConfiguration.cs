using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class DeliveriesConfiguration : IEntityTypeConfiguration<Deliveries>
{
    public void Configure(EntityTypeBuilder<Deliveries> builder)
    {
        builder.HasKey(del => del.Id);

        builder
            .HasOne(del => del.Provider)
            .WithMany(p => p.Deliveries)
            .HasForeignKey(del => del.ProviderId);

        builder.HasMany(del => del.Invoices)
            .WithOne(inv => inv.Delivery)
            .HasForeignKey(inv => inv.DeliveryId);
    }
}
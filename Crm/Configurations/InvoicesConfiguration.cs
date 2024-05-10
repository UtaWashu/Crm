using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class InvoicesConfiguration : IEntityTypeConfiguration<Invoices>
{
    public void Configure(EntityTypeBuilder<Invoices> builder)
    {
        builder.HasKey(inv => inv.Id);

        builder
            .HasOne(inv => inv.Delivery)
            .WithMany(del => del.Invoices)
            .HasForeignKey(inv => inv.DeliveryId);

        builder
            .HasOne(inv => inv.Product)
            .WithMany(pr => pr.Invoices)
            .HasForeignKey(inv => inv.ProductId);

    }
}
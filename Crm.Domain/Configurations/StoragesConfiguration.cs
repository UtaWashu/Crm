using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class StoragesConfiguration : IEntityTypeConfiguration<Storages>
{
    public void Configure(EntityTypeBuilder<Storages> builder)
    {
        builder.HasKey(s => s.Id);

        builder
            .HasOne(s => s.Product)
            .WithMany(p => p.Storages)
            .HasForeignKey(s => s.ProductId);

    }
}
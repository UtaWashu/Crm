using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class ProvidersConfiguration : IEntityTypeConfiguration<Providers>
{
    public void Configure(EntityTypeBuilder<Providers> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .HasMany(p => p.Deliveries)
            .WithOne(d => d.Provider)
            .HasForeignKey(d => d.ProviderId);

    }
}
using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class LoyalitiesConfiguration : IEntityTypeConfiguration<Loyalities>
{
    public void Configure(EntityTypeBuilder<Loyalities> builder)
    {
        builder.HasKey(l => l.Id);

        builder
            .HasMany(l => l.Clients)
            .WithOne(c => c.Loality)
            .HasForeignKey(c => c.LoyalityId);
    }
}
using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class ChecksConfiguration : IEntityTypeConfiguration<Checks>
{
    public void Configure(EntityTypeBuilder<Checks> builder)
    {
        builder.HasKey(ch => ch.Id);

        builder
            .HasOne(ch => ch.Order)
            .WithMany(o => o.Checks)
            .HasForeignKey(ch => ch.OrderId);

        builder
            .HasOne(ch => ch.Dish)
            .WithMany(d => d.Checks)
            .HasForeignKey(ch => ch.OrderId);
    }
}
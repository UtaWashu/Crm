using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class DishesConfiguration : IEntityTypeConfiguration<Dishes>
{
    public void Configure(EntityTypeBuilder<Dishes> builder)
    {
        builder.HasKey(d => d.Id);

        builder
            .HasOne(d => d.Category)
            .WithMany(dc => dc.Dishes)
            .HasForeignKey(d => d.CategoryId);

        builder
            .HasMany(d => d.DishComponents)
            .WithOne(dc => dc.Dish)
            .HasForeignKey(dc => dc.DishId);

        builder
            .HasMany(d => d.Checks)
            .WithOne(ch => ch.Dish)
            .HasForeignKey(ch => ch.DishId);
    }
}
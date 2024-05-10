using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class DishComponentsConfiguration : IEntityTypeConfiguration<DishComponents>
{
    public void Configure(EntityTypeBuilder<DishComponents> builder)
    {
        builder.HasKey(dc => dc.Id);

        builder
            .HasOne(dc => dc.Dish)
            .WithMany(d => d.DishComponents)
            .HasForeignKey(dc => dc.DishId);

        builder
            .HasOne(dc => dc.Product)
            .WithMany(p => p.DishComponents)
            .HasForeignKey(dc => dc.ProductId);
    }
}
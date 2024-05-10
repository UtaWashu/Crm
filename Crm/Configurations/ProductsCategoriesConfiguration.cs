using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class ProductsCategoriesConfiguration : IEntityTypeConfiguration<ProductCategories>
{
    public void Configure(EntityTypeBuilder<ProductCategories> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder
            .HasMany(pr => pr.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

    }
}
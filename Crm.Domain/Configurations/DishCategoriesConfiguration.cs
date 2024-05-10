using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations
{
    public class DishCategoriesConfiguration : IEntityTypeConfiguration<DishCategories>
    {
        public void Configure(EntityTypeBuilder<DishCategories> builder)
        {
            builder.HasKey(dc => dc.Id);

            builder
                .HasMany(dc => dc.Dishes)
                .WithOne(d => d.Category)
                .HasForeignKey(d => d.CategoryId);
        }
    }
}

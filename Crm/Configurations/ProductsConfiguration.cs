using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations
{
    public class ProductsConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.Category)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.CategoryId);

            builder
                .HasMany(p => p.Invoices)
                .WithOne(inv => inv.Product)
                .HasForeignKey(p => p.ProductId);

            builder
                .HasMany(p => p.Storages)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            builder
                .HasMany(p => p.DishComponents)
                .WithOne(dc => dc.Product)
                .HasForeignKey(dc => dc.ProductId);
        }
    }
}

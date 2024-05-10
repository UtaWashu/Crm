using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasMany(p => p.Orders)
                .WithOne(o => o.Payment)
                .HasForeignKey(o => o.PaymentId);
        }
    }
}

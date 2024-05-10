using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations
{
    public class ClientsConfiguration : IEntityTypeConfiguration<Clients>
    {
        public void Configure(EntityTypeBuilder<Clients> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasOne(c => c.Loality)
                .WithMany(l => l.Clients)
                .HasForeignKey(c => c.LoyalityId);

            builder
                .HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId);
        }
    }
}

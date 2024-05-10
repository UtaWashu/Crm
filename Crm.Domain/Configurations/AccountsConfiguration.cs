using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations
{
    public class AccountsConfiguration : IEntityTypeConfiguration<Accounts>
    {
        public void Configure(EntityTypeBuilder<Accounts> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .HasOne(e => e.Employee)
                .WithOne(a => a.Account)
                .HasForeignKey<Employees>(e => e.Id);
        }
    }
}

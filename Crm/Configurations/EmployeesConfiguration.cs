using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class EmployeesConfiguration : IEntityTypeConfiguration<Employees>
{
    public void Configure(EntityTypeBuilder<Employees> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .HasOne(e => e.Account)
            .WithOne(a => a.Employee)
            .HasForeignKey<Accounts>(a => a.Id);

        builder
            .HasOne(e => e.Role)
            .WithMany(er => er.Employees)
            .HasForeignKey(er => er.RoleId);

        builder
            .HasMany(e => e.Orders)
            .WithOne(o => o.Employee)
            .HasForeignKey(o => o.EmployeeId);
    }
}
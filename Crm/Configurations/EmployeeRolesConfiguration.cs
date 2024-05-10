using Crm.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crm.Domain.Configurations;

public class EmployeeRolesConfiguration : IEntityTypeConfiguration<EmployeeRoles>
{
    public void Configure(EntityTypeBuilder<EmployeeRoles> builder)
    {
        builder.HasKey(er => er.Id);

        builder
            .HasMany(er => er.Employees)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.RoleId);
    }
}
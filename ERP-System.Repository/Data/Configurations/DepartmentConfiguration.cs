using ERP_System.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Repository.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Department - Manager Relationship
            builder
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull); // If manager is removed, set it as null.
            builder
            .Property(d => d.ManagerId)
            .HasConversion(
                v => v == 0 ? (int?)null : v,  // Convert 0 to NULL before saving
                v => v ?? 0                   // Convert NULL to 0 when retrieving (optional)
            );
        }
    }
}

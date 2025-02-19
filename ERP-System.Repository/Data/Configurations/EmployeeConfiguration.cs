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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
                builder
           .HasOne(e => e.Department)
           .WithMany(d => d.Employees)
           .HasForeignKey(e => e.DepartmentId)
           .OnDelete(DeleteBehavior.Restrict);

            // Employee - Manager (Self-Referencing Relationship)
            builder
                .HasOne(e => e.Manager)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting manager if employees exist.
        }
    }
}

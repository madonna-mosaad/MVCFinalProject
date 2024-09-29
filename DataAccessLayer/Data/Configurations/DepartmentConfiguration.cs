using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            //this is Server-Side validations (ely betcheck 3leha fe (ModelState.IsValid))
            builder.HasMany(D=>D.Employees).WithOne(E=>E.Department).OnDelete(DeleteBehavior.Cascade);
            builder.Property(D => D.Code).IsRequired();
            builder.Property(D=>D.Name).IsRequired();
        }
    }
}

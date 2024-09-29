using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //this is Server-Side validations (ely betcheck 3leha fe (ModelState.IsValid))
            builder.Property(E => E.Salary).HasColumnType("decimal(10,2)");
            builder.Property(E=>E.Gender).HasConversion(G=>G.ToString(),G=>(Gender)Enum.Parse(typeof(Gender),G,false));
            builder.Property(E => E.Name).IsRequired().HasMaxLength(50);
        }
    }
}

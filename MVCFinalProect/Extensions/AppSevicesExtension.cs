using BusinessLayer.Interfaces;
using BusinessLayer.Repository;
using DataAccessLayer.Data.Contexts;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC.Helpers;


namespace MVC.Extentions
{
    public static class AppSevicesExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(p => p.AddProfile(new MappingProfiles()));

            //in GenericRepository before use UnitOfWork
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            //after use UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddIdentity<Users, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddTransient<EmailSetting>();
            
			return services;
        }
    }
}

using DataAccessLayer.Data.Contexts;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //to any service work with dependency injection
            #region ConfigurServices
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddServices();
            //External login
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(op =>
            {
                IConfiguration configuration = builder.Configuration.GetSection("Authentication:Google");
                op.ClientId = configuration["ClientId"];
                op.ClientSecret = configuration["ClientSecret"];
            });

            #endregion
            var app=builder.Build();
            //to any middle ways 
            #region Configure
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=SignIn}/{id?}");
            });
            #endregion
            app.Run();
        }


    }
}

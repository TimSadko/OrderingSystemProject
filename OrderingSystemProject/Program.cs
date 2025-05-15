using OrderingSystemProject.Controllers;
using OrderingSystemProject.Other;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
         
            Hasher.SetSalt(builder.Configuration.GetSection("Salt").Value); // Get salt from appsetting.json file and give it to hasher (used for hashing passwords)
            //Console.WriteLine($"salt: {builder.Configuration.GetSection("Salt").Value}"); // Print salt to console
            //Console.WriteLine($"pass0: {Hasher.GetHashString("cook")}"); // Print hashed value to console

            // Add databases
            builder.Services.AddSingleton<IEmployeeDB, EmployeeDB>(); // Add db to services
            builder.Services.AddScoped<IEmployeeServices, EmployeeService>(); // Add service to services

            builder.Services.AddSingleton<IOrderDB, OrderDB>(); // Add db to services
            builder.Services.AddScoped<IOrderServices, OrderServices>(); // Add service to services

            builder.Services.AddSingleton<IOrderItemDB, OrderItemDB>(); // Add db to services
            builder.Services.AddScoped<IOrderItemServices, OrderItemServices>(); // Add service to services

            builder.Services.AddSingleton<IMenuItemDB, MenuItemDB>(); // Add db to services
            builder.Services.AddScoped<IMenuItemServices, MenuItemServices>(); // Add service to services

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options => // Configure sessions
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession(); // Enable sessions

			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

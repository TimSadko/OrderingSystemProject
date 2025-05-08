using OrderingSystemProject.Controllers;
using OrderingSystemProject.Other;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Console.WriteLine($"pass: {Hasher.GetHashString("test")}");

            // Add databases
            DefaultConfiguration def = new DefaultConfiguration(builder.Configuration.GetConnectionString("OrderingDatabase"));

            var _employee_rep = new EmployeeDB(def);
            builder.Services.AddSingleton<IEmployeeDB>(_employee_rep);
            CommonController._employee_rep = _employee_rep;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

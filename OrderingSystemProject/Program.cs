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
         
            Hasher.SetSalt(builder.Configuration.GetSection("Salt").Value); // Get salt from appsetting.json file and give it to hasher (used for hashing passwords)
            //Console.WriteLine($"salt: {builder.Configuration.GetSection("Salt").Value}"); // Print salt to console
            //Console.WriteLine($"pass0: {Hasher.GetHashString("cook")}"); // Print hashed value to console

            // Add databases
            DefaultConfiguration def = new DefaultConfiguration(builder.Configuration.GetConnectionString("OrderingDatabase")); // Create custom configuration and give it connection string

            var _employee_rep = new EmployeeDB(def); // Create DB
            builder.Services.AddSingleton<IEmployeeDB>(_employee_rep); // Add it to services
            CommonController._employee_rep = _employee_rep; // Add it to common conrtroller

            var _order_rep = new OrderDB(def); // Create DB
            builder.Services.AddSingleton<IOrderDB>(_order_rep); // Add it to services
            CommonController._order_rep = _order_rep; // Add it to common conrtroller

            var _menu_item_rep = new MenuItemDB(def); // Create DB
            builder.Services.AddSingleton<IMenuItemDB>(_menu_item_rep); // Add it to services
            CommonController._menu_item_rep = _menu_item_rep; // Add it to common conrtroller

            var _order_item_rep = new OrderItemDB(def); // Create DB
            builder.Services.AddSingleton<IOrderItemDB>(_order_item_rep); // Add it to services
            CommonController._order_item_rep = _order_item_rep; // Add it to common conrtroller

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


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
            Console.WriteLine($"pass0: {Hasher.GetHashString("waiter")}"); // Print hashed value to console

            var _employee_rep = new DBEmployeesRepository(builder.Configuration);
            builder.Services.AddSingleton<IEmployeesRepository>(_employee_rep);         
            CommonRepository._employee_rep = _employee_rep;

            builder.Services.AddScoped<IEmployeesService, EmployeesService>();


            var _order_rep = new DbOrdersRepository(builder.Configuration);
            builder.Services.AddSingleton<IOrdersRepository>(_order_rep);
            CommonRepository._order_rep = _order_rep;

            builder.Services.AddScoped<IOrderServices, OrderServices>();


            var _menu_item_rep = new DbMenuItemsRepository(builder.Configuration);
            builder.Services.AddSingleton<IMenuItemsRepository>(_menu_item_rep);
            CommonRepository._menu_item_rep = _menu_item_rep;

            builder.Services.AddScoped<IMenuItemServices, MenuItemServices>();


            var _order_item_rep = new DbOrderItemsRepository(builder.Configuration);
            builder.Services.AddSingleton<IOrderItemsRepository>(_order_item_rep);
            CommonRepository._order_item_rep = _order_item_rep;

            builder.Services.AddScoped<IOrderItemServices, OrderItemServices>();


            var _payment_rep = new DbPaymentRepository(builder.Configuration);
            builder.Services.AddSingleton<IPaymentRepository>(_payment_rep);
            CommonRepository._payment_rep = _payment_rep;

            //builder.Services.AddScoped<IOrderItemServices, OrderItemServices>(); // To do service!

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
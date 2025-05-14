using OrderingSystemProject.Controllers;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;


namespace OrderingSystemProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Console.WriteLine($"pass: {Hasher.GetHashString("test")}");
            
            // Add services to the container.
            builder.Services.AddSingleton<IEmployeesRepository, DbEmployeesRepository>();
            builder.Services.AddScoped<IEmployeesService, EmployeesService>();
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

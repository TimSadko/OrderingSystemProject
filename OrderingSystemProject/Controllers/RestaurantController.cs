using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;

namespace OrderingSystemProject.Controllers;

public class RestaurantController : Controller
{
    private readonly ITablesServices _tablesServices;

    public RestaurantController(ITablesServices tablesServices)
    {
        _tablesServices = tablesServices;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Overview()
    {
        // check authorization using static helper
        if (!Authorization.IsUserLoggedIn(HttpContext)) return RedirectToAction("Login", "Employees");
        
        try
        {
            // get all tables with and with no order
            List<Table> tables = _tablesServices.GetAllTablesWithOrders();
            
            return View(tables);
        }
        catch (Exception ex)
        {
            ViewData["Exception"] = "An error occurred. Please try again." + ex.Message;
            // return an empty list of tables
            return View(new List<Table>());
        }
    }

    [HttpPost]
    public IActionResult Overview(Table table)
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult RefreshOverview()
    {
        try
        {
            // set success message
            TempData["SuccessMessage"] = "Tables refreshed at " + DateTime.Now.ToString("HH:mm:ss");
        
            // refreshing by redirecting to Overview 
            return RedirectToAction("Overview");
        }
        catch (Exception ex)
        {
            // set error message
            ViewData["Exception"] = "Error refreshing tables: " + ex.Message;
            return RedirectToAction("Overview");
        }
    }
}
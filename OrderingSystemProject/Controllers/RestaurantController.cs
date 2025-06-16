using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers;

public class RestaurantController : Controller
{
    private readonly ITablesService _tablesService;
    private readonly IOrderService _ordersService;

    public RestaurantController(ITablesService tablesService, IOrderService ordersService)
    {
        _tablesService = tablesService;
        _ordersService = ordersService;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Overview()
    {
        EmployeeType? userRole = Authorization.GetUserRole(HttpContext);

        // check if user is logged in and has correct role
        if (userRole != EmployeeType.Waiter && userRole != EmployeeType.Manager) return RedirectToAction("Login", "Employees");

        try
        {
            // get all tables with and with no order
            List<Table> tables = _tablesService.GetAllTables();
            List<Order> activeOrders = _ordersService.GetActiveOrders();

            Dictionary<int, Order> ordersDictionary = new Dictionary<int, Order>();
            foreach (Order order in activeOrders)
            {
                ordersDictionary[order.TableId] = order;
            }
            // create view model
            RestaurantOverviewViewModel viewModel = new RestaurantOverviewViewModel(tables, ordersDictionary);

            // return view with view model
            return View(viewModel);
        }
        catch (Exception ex)
        {
            ViewData["Exception"] = "An error occurred. Please try again." + ex.Message;
            // return empty view model in case of error
            return View(new RestaurantOverviewViewModel());
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
            TempData["Exception"] = "Error refreshing tables: " + ex.Message;
            return RedirectToAction("Overview");
        }
    }
    
    [HttpGet]
    public IActionResult FreeTable(int tableId)
    {
        bool isStatusChanged = _tablesService.ChangeTableStatus(tableId, TableStatus.Available);
    
        if (!isStatusChanged)
        {
            TempData["ErrorMessage"] = "Cannot change table status - table has active orders";
            return RedirectToAction("Overview");
        }
        return RedirectToAction("RefreshOverview");
    }

    [HttpGet]
    public IActionResult OccupyTable(int tableId)
    {
        bool isStatusChanged = _tablesService.ChangeTableStatus(tableId, TableStatus.Occupied);
        
        if (!isStatusChanged)
        {
            TempData["ErrorMessage"] = "Cannot change table status";
            return RedirectToAction("Overview"); 
        }
        return RedirectToAction("RefreshOverview");
    }
    
    [HttpGet]
    public IActionResult MarkAsServed(int orderId, string itemType)
    {
        try
        {
            if (_ordersService.MarkOrderAsServed(orderId, itemType))
                TempData["SuccessMessage"] = $"{itemType} marked as served successfully!";
            else
                TempData["ErrorMessage"] = $"Could not mark {itemType} as served.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error updating order status: " + ex.Message;
        }
        return RedirectToAction("RefreshOverview");
    }
}
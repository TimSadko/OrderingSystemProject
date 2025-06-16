using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers;

public class RestaurantController : Controller
{
    private readonly ITablesService _tablesService;
    private readonly IOrdersService _ordersService;

    public RestaurantController(ITablesService tablesService, IOrdersService ordersService)
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

    //[HttpPost("UpdateTableStatus")]
    //public IActionResult UpdateTableStatus(int tableId, TableStatus status)
    //{
    //    try
    //    {
    //        if (tableId <= 0)
    //        {
    //            TempData["ErrorMessage"] = "Invalid table selection";
    //            return RedirectToAction(nameof(Overview));
    //        }

    //        bool success = _tablesService.UpdateTableStatus(tableId, status);

    //        if (success)
    //        {
    //            TempData["SuccessMessage"] = $"Table {tableId} status updated to {status}";
    //        }
    //        else
    //        {
    //            TempData["ErrorMessage"] = $"Failed to update table {tableId} status";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
    //    }

    //    return RedirectToAction(nameof(Overview));
    //}

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
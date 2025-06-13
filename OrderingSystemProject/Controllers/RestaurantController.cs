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
    private readonly IOrdersRepository _ordersRepository;

    public RestaurantController(ITablesService tablesService, IOrdersRepository ordersRepository)
    {
        _tablesService = tablesService;
        _ordersRepository = ordersRepository;
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
            List<Order> activeOrders = _ordersRepository.GetActiveOrders();
            
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
}
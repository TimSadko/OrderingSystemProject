using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers
{
    public class WaiterController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ITablesServices _tablesService;
        private WaiterViewModel _waiterViewModel;

        public WaiterController(IMenuItemService menuItemService, ITablesServices tablesService)
        {
            _menuItemService = menuItemService;
            _tablesService = tablesService;

            _waiterViewModel = new WaiterViewModel();
            _waiterViewModel.MenuItems = CommonRepository._menu_item_rep.GetAll();
            _waiterViewModel.Cart = new List<OrderItem>();

            Console.WriteLine("Create");
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine($"{_waiterViewModel.Cart.Count}");
            return View(_waiterViewModel);
        }

        [HttpGet]
        public IActionResult AddItem(int itemId)
        {

            // Add To item list
            var orderItem = new OrderItem(-1, -1, itemId, 1, "", 0);
            orderItem.MenuItem = CommonRepository._menu_item_rep.GetById(itemId);

            _waiterViewModel.Cart.Add(orderItem);

            return View(_waiterViewModel);
        }

        public IActionResult Tables()
        {
            var tables = _tablesService.GetAllTables();
            return View(tables); // Передаємо список столів у View
        }

        [HttpPost]
        public IActionResult SelectTable(int tableNumber)
        {
            return RedirectToAction("Index", new { tableNumber });
        }
    }
}

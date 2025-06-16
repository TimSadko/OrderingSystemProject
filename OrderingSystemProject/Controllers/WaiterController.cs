using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;
using System.Reflection;

namespace OrderingSystemProject.Controllers
{
    public class WaiterController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ITablesService _tablesService;
        private WaiterViewModel _waiterViewModel;
        private readonly IOrderService _orderService;

        public WaiterController(IMenuItemService menuItemService, ITablesService tablesService, IOrderService orderService)
        {
            _menuItemService = menuItemService;
            _tablesService = tablesService;
            _orderService = orderService;
        }

        [HttpGet("Waiter/Index/{tableId}")]
        public IActionResult Index(int? tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                if (tableId == null)            // No table selected, redirect to restaurant overview.
                {
                    return RedirectToAction("Overview", "Restaurant");
                }
                var table = _tablesService.GetTableByNumber((int)tableId);
                if (table == null)
                {
                    throw new Exception("Invalid Table ID");
                }

                // Build view model with current table, full menu and cart 
                var model = new WaiterViewModel
                {
                    Table = _tablesService.GetTableByNumber((int)tableId),
                    MenuItems = _menuItemService.GetAll(),
                    Cart = _menuItemService.GetCart(),
                };
              
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex.Message;
                return RedirectToAction("Overview", "Restaurant");
            }
        }

        [HttpPost]
        public IActionResult AddItem(int itemId, int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                // Add item to cart using service.
                _menuItemService.AddItem(itemId);
                var addedItem = _menuItemService.GetById(itemId);

                TempData["SuccessMessage"] = $"Item {addedItem.Name} was added to the Order!";
                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                ViewData["Exception"] = $"Error with adding item to the Order: {e.Message}";
                return RedirectToAction("Index", new { tableId });
            }
        }


        [HttpPost]
        public IActionResult IncreaseQuantity(int itemId, int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                _menuItemService.IncreaseQuantity(itemId);

                var increaseQ = _menuItemService.GetById(itemId);
                TempData["SuccessMessage"] = $"Amount of '{increaseQ.Name}' was increased by 1";

                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error with increasing amount: {e.Message}";

                return RedirectToAction("Index", new { tableId });
            }
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int itemId, int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                _menuItemService.DecreaseQuantity(itemId);

                var decreaseQ = _menuItemService.GetById(itemId);
                TempData["SuccessMessage"] = $"Amount of '{decreaseQ.Name}' was deacresed by 1";

                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error with decreasing amount: {e.Message}";

                return RedirectToAction("Index", new { tableId });
            }
        }


        [HttpPost]
        public IActionResult RemoveItem(int itemId, int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                _menuItemService.RemoveItem(itemId);

                var removedItem = _menuItemService.GetById(itemId);
                TempData["SuccessMessage"] = $"Item '{removedItem.Name}' was removed!";

                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error with removing item: {e.Message}";

                return RedirectToAction("Index", new { tableId });
            }
        }

        [HttpPost]
        public IActionResult UpdateComment(int itemId, string comment, int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                _menuItemService.UpdateComment(itemId, comment);
                var updatedItem = _menuItemService.GetById(itemId);

                TempData["SuccessMessage"] = $"Comment '{comment}' was added to {updatedItem.Name}!";
                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error updating comment: {e.Message}";

                return RedirectToAction("Index", new { tableId });
            }
        }

        [HttpPost]
        public IActionResult ClearOrder(int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                // Clear all items from cart
                _menuItemService.CancelOrder();

                TempData["SuccessMessage"] = $"Order was Cancelled!";
                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error with Order canceling: {e.Message}";

                return RedirectToAction("Index", new { tableId });
            }
        }

        [HttpGet]
        public IActionResult ReviewOrder(int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                // Load full cart and table info for review
                var model = new WaiterViewModel
                {
                    Table = _tablesService.GetTableByNumber(tableId),
                    MenuItems = _menuItemService.GetAll(),
                    Cart = _menuItemService.GetCart()
                };

                return View(model);
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error with Order Review: {e.Message}";

                return View();
            }
        }

        [HttpPost]
        public IActionResult SendToKitchen(int tableId)
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                // Get current cart items
                var orderedItems = _menuItemService.GetCart();

                // Create the order and persist it via service
                var order = _orderService.CreateOrder(tableId, orderedItems);

                // Cleaning cart after sending 
                _menuItemService.CancelOrder();

                TempData["SuccessMessage"] = $"Order №{order.OrderId} was successfully sent to the Kitchen!";
                return RedirectToAction("ReviewOrder", new { tableId });
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Order Error: {e.Message}";
                return RedirectToAction("ReviewOrder", new { tableId });
            }
        }
        
        public IActionResult Filter(
            MenuManagementViewModel.CardFilterType cardFilterType,
            MenuManagementViewModel.CategoryFilterType categoryFilterType, int tableId
        )
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
                // Filter menu items based on category and card 
                List<MenuItem> menuItems = _menuItemService.Filter(categoryFilterType, cardFilterType);

                var model = new WaiterViewModel
                {
                    CardFilter = cardFilterType,
                    CategoryFilter = categoryFilterType,
                    Table = _tablesService.GetTableByNumber((int)tableId),
                    MenuItems = menuItems,
                    Cart = _menuItemService.GetCart(),
                };

                return View(nameof(Index), model);
            }
            catch (Exception e)
            {
                ViewData["Exception"] = $"Exception occured: {e.Message}";
                return View();
            }
        }

        // Checks logged-in user 
        private bool Authenticate()
        {
            var user_role = Authorization.GetUserRole(this.HttpContext);

            if (user_role != null && (user_role == EmployeeType.Waiter || user_role == EmployeeType.Manager)) return true;

            return false;
        }
    }
}

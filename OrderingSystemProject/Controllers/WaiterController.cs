using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;

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
                if (tableId == null)
                {
                    return RedirectToAction("Overview", "Restaurant");
                }
                var table = _tablesService.GetTableByNumber((int)tableId);
                if (table == null)
                {
                    throw new Exception("Invalid Table ID");
                }


                var model = new WaiterViewModel
                {
                    Table = _tablesService.GetTableByNumber((int)tableId),
                    MenuItems = _menuItemService.GetAll(),
                    Cart = _menuItemService.GetCart(),
                    //CardFilter = MenuManagementViewModel.CardFilterType.ALL;
                    //CategoryFilter = MenuManagementViewModel.CategoryFilterType.ALL;
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
                var orderedItems = _menuItemService.GetCart();
                
                var order = _orderService.CreateOrder(tableId, orderedItems);

                // Cleaning Cart
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
            CardFilterType cardFilterType,
            CategoryFilterType categoryFilterType, 
            int tableId
        )
        {
            if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
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

        private bool Authenticate()
        {
            var user_role = Authorization.GetUserRole(this.HttpContext);

            if (user_role == EmployeeType.Waiter) return true;

            return false;
        }
    }
}

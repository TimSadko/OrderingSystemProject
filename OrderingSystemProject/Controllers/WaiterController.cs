using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers
{
    public class WaiterController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ITablesServices _tablesService;
        private WaiterViewModel _waiterViewModel;
        private readonly IOrderService _orderService;

        public WaiterController(IMenuItemService menuItemService, ITablesServices tablesService, IOrderService orderService)
        {
            _menuItemService = menuItemService;
            _tablesService = tablesService;
            _orderService = orderService;

            _waiterViewModel = new WaiterViewModel();
            _waiterViewModel.MenuItems = CommonRepository._menu_item_rep.GetAll();
            _waiterViewModel.Cart = new List<OrderItem>();
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    Console.WriteLine($"{_waiterViewModel.MenuItems}");
        //    Console.WriteLine($"{_waiterViewModel.Cart.Count}");
        //    return View(_waiterViewModel);
        //}

        [HttpGet]
        public IActionResult Index()
        {
            var model = new WaiterViewModel
            {
                MenuItems = _menuItemService.GetAll(),
                Cart = _menuItemService.GetCart()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddItem(int itemId)
        {
            try
            {
                _menuItemService.AddItem(itemId);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Error adding item: {e.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult IncreaseQuantity(int itemId)
        {
            _menuItemService.IncreaseQuantity(itemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int itemId)
        {
            _menuItemService.DecreaseQuantity(itemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveItem(int itemId)
        {
            _menuItemService.RemoveItem(itemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateComment(int itemId, string comment)
        {
            try
            {
                _menuItemService.UpdateComment(itemId, comment);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Error updating comment: {e.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult ClearOrder()
        {
            _menuItemService.CancelOrder();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SendToKitchen(int tableId, List<OrderItem> items)
        {
            try
            {
                if (items == null || items.Count == 0)
                {
                    return RedirectToAction("Index");
                }

                var order = _orderService.CreateOrder(tableId, items);

                TempData["SuccessMessage"] = $"Order №{order.OrderId} was successfully sent to the Kitchen!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Order Error: {e.Message}";
                return RedirectToAction("Index");
            }
        }


        //[HttpGet("Waiter/AddItem/{itemId}")]
        //public IActionResult AddItem(int itemId)
        //{
        //    try
        //    {
        //        // Add To item list
        //        var orderItem = new OrderItem(-1, -1, itemId, 1, "", 0);
        //        orderItem.MenuItem = CommonRepository._menu_item_rep.GetById(itemId);

        //        _waiterViewModel.Cart.Add(orderItem);

        //        return View("Index", _waiterViewModel);
        //    }


        //    catch (Exception e)
        //    {
        //        ViewData["Exception"] = $"Exception occured: {e.Message}";
        //        return Index();
        //    }
        //}

        [HttpPost]
        public IActionResult SubmitOrder(int tableId)
        {
            var cartItems = _menuItemService.GetCart();
            var order = _orderService.CreateOrder(tableId, cartItems);

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        public IActionResult Tables()
        {
            var tables = _tablesService.GetAllTables();
            return View(tables);
        }

        [HttpPost]
        public IActionResult SelectTable(int tableNumber)
        {
            return RedirectToAction("Index", new { tableNumber });
        }
        
        public IActionResult Filter(
            MenuManagementViewModel.CardFilterType cardFilterType,
            MenuManagementViewModel.CategoryFilterType categoryFilterType
        )
        {
            try
            {
                List<MenuItem> menuItems = _menuItemService.Filter(categoryFilterType, cardFilterType);
                var menuManagementViewMode = new MenuManagementViewModel(
                    menuItems,
                    cardFilterType,
                    categoryFilterType
                );
                return View(nameof(Index), menuManagementViewMode);
            }
            catch (Exception e)
            {
                ViewData["Exception"] = $"Exception occured: {e.Message}";
                return Index();
            }
        }
    }
}

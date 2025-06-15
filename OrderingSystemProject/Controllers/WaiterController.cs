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

        [HttpGet("Waiter/Index/{tableId}")]
        public IActionResult Index(int? tableId)
        {
            try
            {
                if (tableId == null)
                {
                    return RedirectToAction("Overview", "Restaurant");
                }
                var table = _tablesService.GetTableById((int)tableId);
                if (table == null)
                {
                    throw new Exception("Invalid Table ID");
                }


                var model = new WaiterViewModel
                {
                    Table = _tablesService.GetTableById((int)tableId),
                    MenuItems = _menuItemService.GetAll(),
                    Cart = _menuItemService.GetCart()
                };
              
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message;
                return RedirectToAction("Overview", "Restaurant");
            }
        }

        [HttpPost]
        public IActionResult AddItem(int itemId, int tableId)
        {
            try
            {
                _menuItemService.AddItem(itemId);
                var addedItem = _menuItemService.GetById(itemId);

                TempData["SuccessMessage"] = $"Item {addedItem.Name} was added to the Order!";
                return RedirectToAction("Index", new { tableId });
            }
            catch (Exception e)
            {
                TempData["Exception"] = $"Error with adding item to the Order: {e.Message}";
                return RedirectToAction("Index", new { tableId });
            }
        }


        [HttpPost]
        public IActionResult IncreaseQuantity(int itemId, int tableId)
        {
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

        [HttpGet]
        public IActionResult ReviewOrder(int tableId)
        {
            try
            {
                var model = new WaiterViewModel
                {
                    Table = _tablesService.GetTableById(tableId),
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
                TempData["Exception"] = $"Order Error: {e.Message}";
                return RedirectToAction("ReviewOrder", new { tableId });
            }
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

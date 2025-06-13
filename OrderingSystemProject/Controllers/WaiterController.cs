using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers
{
    public class WaiterController : Controller
    {
        private IMenuItemService _menuItemService;

        public WaiterController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            EmployeeType? userRole = Authorization.GetUserRole(HttpContext);
            // check if user is logged in and has correct role
            if (userRole != EmployeeType.Waiter && userRole != EmployeeType.Manager) return RedirectToAction("Login", "Employees");

            List<MenuItem> menuItems = _menuItemService.GetAll();
            var menuManagementViewMode = new MenuManagementViewModel(
                menuItems,
                MenuManagementViewModel.CardFilterType.ALL,
                MenuManagementViewModel.CategoryFilterType.ALL
            );
            return View(menuManagementViewMode);
        }

        [HttpGet]
        public IActionResult AddItem()
        {

            // Ad To item list

            return RedirectToAction("Index");
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

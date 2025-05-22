using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
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
    }
}

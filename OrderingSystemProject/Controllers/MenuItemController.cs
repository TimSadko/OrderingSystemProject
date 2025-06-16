using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers;

public class MenuItemController : Controller
{
    private IMenuItemService _menuItemService;

    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        List<MenuItem> menuItems = _menuItemService.GetAll();
        var menuManagementViewMode = new MenuManagementViewModel(
            menuItems,
            MenuManagementViewModel.CardFilterType.ALL,
            MenuManagementViewModel.CategoryFilterType.ALL
        );
        return View(menuManagementViewMode);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(MenuItem menuItem)
    {
        try
        {
            _menuItemService.Add(menuItem);
            TempData["SuccessMessage"] = "Menu item has been added!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(menuItem);
        }
    }

    [HttpGet("MenuItem/Delete/{menuItemId}")]
    public ActionResult Delete(int? menuItemId)
    {
        if (menuItemId == null)
        {
            return NotFound();
        }

        MenuItem? menuItem = _menuItemService.GetById((int)menuItemId);
        return View(menuItem);
    }

    [HttpPost]
    public ActionResult Delete(MenuItem menuItem)
    {
        try
        {
            _menuItemService.Delete(menuItem);
            TempData["SuccessMessage"] = "Menu item has been removed!";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(menuItem);
        }
    }

    [HttpGet("MenuItem/Edit/{menuItemId}")]
    public IActionResult Edit(int? menuItemId)
    {
        if (menuItemId == null)
        {
            return NotFound();
        }
        var menuItem = _menuItemService.GetById((int)menuItemId);
        return View(menuItem);
    }

    [HttpPost]
    public IActionResult Edit(MenuItem menuItem)
    {
        try
        {
            _menuItemService.Update(menuItem);
            TempData["SuccessMessage"] = "Menu item has been updated!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(menuItem);
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

    [HttpPost]
    public IActionResult Activate(int menuItemId)
    {
        try
        {
            _menuItemService.Activate(menuItemId);
            TempData["SuccessMessage"] = "Menu item has been activated!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public IActionResult Deactivate(int menuItemId)
    {
        try
        {
            _menuItemService.Deactivate(menuItemId);
            TempData["SuccessMessage"] = "Menu item has been deactivated!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    private bool IsUnauthorisedUser()
    {
        var userRole = Authorization.GetUserRole(HttpContext);
        return userRole != EmployeeType.Waiter && userRole != EmployeeType.Manager;
    }
}
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

        try
        {
            List<MenuItem> menuItems = _menuItemService.GetAll();
            var menuManagementViewModel = new MenuManagementViewModel(
                menuItems,
                CardFilterType.ALL,
                CategoryFilterType.ALL
            );
            return View(menuManagementViewModel);
        }
        catch (Exception e)
        {
            TempData["Exception"] = $"Exception occured: {e.Message}";
            var menuManagementViewModel = new MenuManagementViewModel(
                new List<MenuItem>(),
                CardFilterType.ALL,
                CategoryFilterType.ALL
            );
            return View(menuManagementViewModel);
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        if (menuItemId == null)
        {
            return NotFound();
        }

        try
        {
            MenuItem? menuItem = _menuItemService.GetById((int)menuItemId);
            return View(menuItem);
        }
        catch (Exception e)
        {
            TempData["Exception"] = $"Exception occured: {e.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public ActionResult Delete(MenuItem menuItem)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        if (menuItemId == null)
        {
            return NotFound();
        }

        try
        {
            var menuItem = _menuItemService.GetById((int)menuItemId);
            return View(menuItem);
        }
        catch (Exception e)
        {
            TempData["Exception"] = $"Exception occured: {e.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public IActionResult Edit(MenuItem menuItem)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

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

    public IActionResult Filter(CardFilterType cardFilterType, CategoryFilterType categoryFilterType)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            List<MenuItem> menuItems = _menuItemService.Filter(categoryFilterType, cardFilterType);
            var menuManagementViewModel = new MenuManagementViewModel(
                menuItems,
                cardFilterType,
                categoryFilterType
            );
            return View(nameof(Index), menuManagementViewModel);
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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

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
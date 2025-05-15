using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class MenuItemController: Controller
{
    private IMenuItemService _menuItemService;

    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<MenuItem> menuItems = _menuItemService.GetAll();
        return View(menuItems);
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
            TempData["MenuItemOperationConfirmMessage"] = "Menu item has been added!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = $"Exception occured: {e.Message}";
            return View(menuItem);
        }
    }
}
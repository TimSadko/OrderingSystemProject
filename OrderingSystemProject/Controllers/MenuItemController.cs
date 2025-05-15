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

    [HttpGet("MenuItem/Delete/{ItemId}")]
    public ActionResult Delete(int? ItemId)
    {
        if (ItemId == null)
        {
            return NotFound();
        }

        MenuItem? menuItem = _menuItemService.GetById((int)ItemId);
        return View(menuItem);
    }

    [HttpPost]
    public ActionResult Delete(MenuItem menuItem)
    {
        try
        {
            _menuItemService.Delete(menuItem);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return View(menuItem);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
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
        catch (Exception e)
        {
            ViewBag.ErrorMessage = $"Exception occured: {e.Message}";
            return View(menuItem);
        }
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var menuItem = _menuItemService.GetById((int)id);
        return View(menuItem);
    }

    [HttpPost]
    public IActionResult Edit(MenuItem menuItem)
    {
        try
        {
            _menuItemService.Update(menuItem);

            TempData["MenuItemOperationConfirmMessage"] = "Menu item has been updated!";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = $"Exception occured: {e.Message}";

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
            //return View(nameof(Index), lecturers);
            return View(nameof(Index), menuManagementViewMode);
        }
        catch (Exception e)
        {
            /*List<MenuItem> menuItems = _menuItemService.GetAll();
            var menuManagementViewMode = new MenuManagementViewModel(
                menuItems,
                MenuManagementViewModel.CardFilterType.ALL,
                MenuManagementViewModel.CategoryFilterType.ALL
            );
            return View(menuManagementViewMode);*/
            
            ViewBag.ErrorMessage = $"Exception occured: {e.Message}";
            return Index();
            /*List<Lecturer> lecturers = _lecturersRepository.GetAll();
            return View(nameof(Index), lecturers);
            return */
        }
    }
}
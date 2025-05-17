using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class RestaurantController : Controller
{
    private readonly ITablesServices _tablesServices;

    public RestaurantController(ITablesServices tablesServices)
    {
        _tablesServices = tablesServices;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Overview()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Overview(Table table)
    {
        return View();
    }
}
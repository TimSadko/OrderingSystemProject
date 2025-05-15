using Microsoft.AspNetCore.Mvc;

namespace OrderingSystemProject.Controllers;

public class RestaurantController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
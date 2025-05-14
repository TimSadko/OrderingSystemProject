using Microsoft.AspNetCore.Mvc;

namespace OrderingSystemProject.Controllers;

public class RestaurantController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
using Microsoft.AspNetCore.Mvc;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
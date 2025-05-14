using Microsoft.AspNetCore.Mvc;

namespace OrderingSystemProject.Controllers
{
    public class KitchenController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace OrderingSystemProject.Controllers
{
    public class KitchenController1 : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

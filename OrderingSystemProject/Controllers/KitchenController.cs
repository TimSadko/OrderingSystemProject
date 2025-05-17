using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers
{
    public class KitchenController : Controller
    {
        private IKitchenServices _serv;

        public KitchenController(IKitchenServices serv)
        {
            _serv = serv;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers
{
    public class KitchenController : Controller
    {
        private IKitchenServices _serv;

        public KitchenController()
        {
            _serv = new KitchenService();
        }

        [HttpGet]
        public IActionResult Index()
        {
			try
            {
                var list = _serv.GetCookOrders(); // Get list od all current orders

                return View(list); 
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex;
            }

            return View();
        }
    }
}

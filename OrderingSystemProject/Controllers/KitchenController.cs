using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models.Kitchen;
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
            var list = _serv.GetCookOrders(); // Get list od all current orders

            try
            {
                

                KitchenViewModel model = new KitchenViewModel(list, DateTime.Now); // Create ne view model 

                return View(model); 
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex;
            }

            return View();
        }
    }
}

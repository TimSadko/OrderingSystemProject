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
            try
            {
				var list = _serv.GetCookOrders(); // Get list od all current orders

				KitchenViewModel model = new KitchenViewModel(list, DateTime.Now); // Create ne view model 

                return View(model); 
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex;
            }

            return View();
        }

        [HttpGet ("Kitchen/TakeOrder/{_order_id}/{_item_id}")]
        public IActionResult TakeOrder(int _order_id, int _item_id)
        {
            try
            {
                _serv.TakeOrder(_order_id, _item_id);
            }
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

            return RedirectToAction("Index");
		}

		[HttpGet ("Kitchen/FinishOrder/{_order_id}/{_item_id}")]
		public IActionResult FinishOrder(int _order_id, int _item_id)
		{
			try
			{
				_serv.FinishOrder(_order_id, _item_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

        [HttpGet]
        public IActionResult Done()
        {
            try
            {
                var list = _serv.GetDoneCookOrders(); // Get list od all current orders

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

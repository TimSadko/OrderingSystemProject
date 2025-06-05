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
				var list = _serv.GetCookOrders();

				KitchenViewModel model = new KitchenViewModel(list, _serv.GetCookOrdersReady(list), DateTime.Now); // Create newS view model 

                return View(model); 
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex;
            }

            return View();
        }

        [HttpGet ("Kitchen/TakeItem/{_order_id}/{_item_id}")]
        public IActionResult TakeItem(int _order_id, int _item_id)
        {
            try
            {
                _serv.TakeItem(_order_id, _item_id);
            }
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

            return RedirectToAction("Index");
		}

		[HttpGet ("Kitchen/FinishItem/{_order_id}/{_item_id}")]
		public IActionResult FinishItem(int _order_id, int _item_id)
		{
			try
			{
				_serv.FinishItem(_order_id, _item_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/ReturnItem/{_order_id}/{_item_id}")]
		public IActionResult ReturnItem(int _order_id, int _item_id)
		{
			try
			{
				_serv.ReturnItem(_order_id, _item_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/TakeOrder/{_order_id}")]
		public IActionResult TakeOrder(int _order_id)
		{
			try
			{
				_serv.TakeOrder(_order_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/FinishOrder/{_order_id}")]
		public IActionResult FinishOrder(int _order_id)
		{
			try
			{
				_serv.FinishOrder(_order_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/ReturnOrder/{_order_id}")]
		public IActionResult ReturnOrder(int _order_id)
		{
			try
			{
				_serv.ReturnOrder(_order_id);
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
                KitchenViewModel model = new KitchenViewModel(_serv.GetDoneCookOrders(), null, DateTime.Now); // Create ne view model 

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

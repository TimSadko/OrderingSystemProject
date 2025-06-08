using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models.Kitchen;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;

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
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

            try
            {
				var list = _serv.GetCookOrders();

				KitchenViewModel model = new KitchenViewModel(list, _serv.GetCookOrdersReady(list), DateTime.Now); // Create newS view model 

                return View(model); 
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex.Message;
            }

            return View();
        }

        [HttpGet ("Kitchen/TakeItem/{_order_id}/{_item_id}")]
        public IActionResult TakeItem(int _order_id, int _item_id)
        {
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
            {
                _serv.TakeItem(_order_id, _item_id);
            }
			catch (Exception ex)
			{
				ViewData["Exception"] = ex.Message;
			}

            return RedirectToAction("Index");
		}

		[HttpGet ("Kitchen/FinishItem/{_order_id}/{_item_id}")]
		public IActionResult FinishItem(int _order_id, int _item_id)
		{
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{
				_serv.FinishItem(_order_id, _item_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex.Message;
			}

			return RedirectToAction("Index");

		}

		[HttpGet("Kitchen/ReturnItem/{_order_id}/{_item_id}")]
		public IActionResult ReturnItem(int _order_id, int _item_id)
		{
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{
				_serv.ReturnItem(_order_id, _item_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex.Message;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/TakeOrder/{_order_id}")]
		public IActionResult TakeOrder(int _order_id)
		{
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

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
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

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
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{
				_serv.ReturnOrder(_order_id);
			}
			catch (Exception ex)
			{
				ViewData["Exception"] = ex.Message;
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
        public IActionResult Done()
        {
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{ 
                KitchenViewModel model = new KitchenViewModel(_serv.GetDoneCookOrders(), null, DateTime.Now); // Create ne view model 

                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex.Message;
            }

            return View();        
        }

		private bool Authenticate()
		{
			var user_role = Authorization.GetUserRole(this.HttpContext);

			if (user_role == Models.EmployeeType.Cook) return true;

			return false;
		}
	}
}

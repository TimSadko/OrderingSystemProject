﻿using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;

namespace OrderingSystemProject.Controllers
{
    public class KitchenController : Controller
    {
        private IKitchenServices _serv;

        public KitchenController(IKitchenServices _serv)
        {
            this._serv = _serv;
        }

        [HttpGet]
        public IActionResult Index()
        {                 
            if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role

            try        
            {
				var list = _serv.GetCookOrders();

				KitchenViewModel model = new KitchenViewModel(list, _serv.GetCookOrdersReady(list), DateTime.Now); // Create new view model 

				//LogOrdersConsole(list);

				TempData["LastUpdate"] = model.LastUpdate.ToString("HH:mm:ss");

                return View(model); 
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message;
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
				TempData["Exception"] = ex.Message;
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
				TempData["Exception"] = ex.Message;
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
				TempData["Exception"] = ex.Message;
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
				TempData["Exception"] = ex;
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
				TempData["Exception"] = ex;
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
				TempData["Exception"] = ex.Message;
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

				//LogOrdersConsole(model.Orders);

				TempData["LastUpdate"] = model.LastUpdate.ToString("HH:mm:ss");

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message;
            }

            return View();        
        }

		[HttpGet("Kitchen/TakeCat/{_order_id}/{cat}")]
		public IActionResult TakeCat(int _order_id, int cat)
		{
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{
				_serv.TakeCat(_order_id, cat);
			}
			catch (Exception ex)
			{
				TempData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/FinishCat/{_order_id}/{cat}")]
		public IActionResult FinishCat(int _order_id, int cat)
		{
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{
				_serv.FinishCat(_order_id, cat);
			}
			catch (Exception ex)
			{
				TempData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		[HttpGet("Kitchen/ReturnCat/{_order_id}/{cat}")]
		public IActionResult ReturnCat(int _order_id, int cat)
		{
			if (!Authenticate()) return RedirectToAction("Login", "Employees");

			try
			{
				_serv.ReturnCat(_order_id, cat);
			}
			catch (Exception ex)
			{
				TempData["Exception"] = ex;
			}

			return RedirectToAction("Index");
		}

		private bool Authenticate()
		{
			var user_role = Authorization.GetUserRole(this.HttpContext);

			if (user_role != null && (user_role == EmployeeType.Cook || user_role == EmployeeType.Manager)) return true;

			return false;
		}

        private void LogOrdersConsole(List<KitchenOrder> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Console.WriteLine($"Items: ({list[i].KitchenStatus})");
				foreach (var item in list[i].Items)
				{
					Console.WriteLine($"  {item.MenuItem.Name} x{item.Amount} -> {item.ItemStatus}");
				}

				Console.WriteLine(" |\n V");

				if (list[i].ItemStarters.Count > 0) Console.WriteLine("Starters:");
				foreach (var item in list[i].ItemStarters)
				{
					Console.WriteLine($"  {item.MenuItem.Name} x{item.Amount} -> {item.ItemStatus}");
				}

				if (list[i].ItemMains.Count > 0) Console.WriteLine("Mains:");
				foreach (var item in list[i].ItemMains)
				{
					Console.WriteLine($"  {item.MenuItem.Name} x{item.Amount} -> {item.ItemStatus}");
				}

				if (list[i].ItemDeserts.Count > 0) Console.WriteLine("Deserts:");
				foreach (var item in list[i].ItemDeserts)
				{
					Console.WriteLine($"  {item.MenuItem.Name} x{item.Amount} -> {item.ItemStatus}");
				}

				Console.WriteLine("\n");
			}
		}
	}
}

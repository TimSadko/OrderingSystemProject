using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Other;
using System.Diagnostics;

namespace OrderingSystemProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
            //return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                Employee? emp = CommonController._employee_rep.TryLogin(model);

                if (emp == null) throw new FailedToLoginException();

                return GetRedirect(emp.EmployeeType); // Redirect based on user type waiter, manager etc.
            }
            catch (FailedToLoginException ex)
            {
                ViewData["Exception"] = ex;
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! {ex.Message}");
                return View(model);
            }          
        }

        private RedirectToActionResult GetRedirect(EMPLOYEE_TYPE type)
        {
            switch (type)
            {
                case EMPLOYEE_TYPE.WAITER:
                    return RedirectToAction("Privacy", "Home");
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

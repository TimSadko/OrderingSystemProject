using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Other;
using System.Diagnostics;
using System.Text.Json;

namespace OrderingSystemProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string logged_in_key = "logged_in_employee";

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
			string? json = HttpContext.Session.GetString(logged_in_key); // Get logged in employee from sessions

			if (json != null) // if employee is saved in session (have logged in with in last 30 minutes), log him in automaticly 
            {
                Employee? emp = JsonSerializer.Deserialize<Employee>(json); // Get logged in employe from session

                if (emp != null) return GetRedirect(emp.EmployeeType); // if employee session is not null, log him in
			}

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                Employee? emp;
				
				emp = CommonController._employee_rep.TryLogin(model); // Get employee by credentials

				if (emp == null) throw new FailedToLoginException(); // If could not found employee in db throw exeption

				HttpContext.Session.SetString(logged_in_key, JsonSerializer.Serialize(emp)); // Add employee to sessions           

                return GetRedirect(emp.EmployeeType); // Redirect based on user type waiter, manager, etc.
            }
            catch (FailedToLoginException ex)
            {
                ViewData["Exception"] = ex; // Pass exeption to view, that it can show it to user
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
            switch (type) // Returns different page depending on employee type
            {
                case EMPLOYEE_TYPE.WAITER:
                    return RedirectToAction("Privacy", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout() // Called when logout
        {
            HttpContext.Session.Remove(logged_in_key); // Remove current session 

            return RedirectToAction("Index"); // Redirect to home
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

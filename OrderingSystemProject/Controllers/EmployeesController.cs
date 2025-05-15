using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class EmployeesController : Controller
{

    private readonly IEmployeesService _employeesService;

    public EmployeesController(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        try
        {
            Employee employee = _employeesService.GetEmployeeByLoginCredentials(loginModel.Login, loginModel.Password);
        
            // if credentials are incorrect
            if (employee == null)
            {
                ViewData["ErrorMessage"] = "Invalid username or password";
                return View(loginModel);
            }
            return RedirectEmployee((EmployeeType)employee.EmployeeType);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = "An error occurred during login. Please try again.";
            
            return View(loginModel);
        }
    }

    private RedirectToActionResult RedirectEmployee(EmployeeType employeeType)
    {
        switch (employeeType)
        {
            case EmployeeType.Waiter:
                return RedirectToAction("Privacy", "Home");
            case EmployeeType.Bartender:
                return RedirectToAction("Index", "Bar");
        }
        return RedirectToAction("Index", "Home");
    }
}
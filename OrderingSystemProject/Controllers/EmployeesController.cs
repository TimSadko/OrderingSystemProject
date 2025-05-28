using System.Text.Json;
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

    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        // get the employee data from session
        string? employeeJson = HttpContext.Session.GetString("LoggedInEmployee");
        if (employeeJson != null) 
        {
            // converting the JSON string into an Employee object
            Employee? employee = JsonSerializer.Deserialize<Employee>(employeeJson);
            if (employee != null) 
            {
                // if employee exists in session, redirect based on employee type
                return RedirectEmployee(employee.EmployeeType); 
            }
        } 
        // if no session exists, show login form
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        try
        {
            Employee employee = _employeesService.GetEmployeeByLoginCredentials(loginModel.UserName, loginModel.Password);
        
            // if credentials are incorrect
            if (employee == null)
            {
                ViewData["ErrorMessage"] = "Invalid username or password";
                return View(loginModel);
            }
            else
            {
                // remember logged in employee
                string userJson = JsonSerializer.Serialize(employee);
                HttpContext.Session.SetString("LoggedInEmployee", userJson);
                
                // redirect Employee by Type
                return RedirectEmployee(employee.EmployeeType);
            }
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = "An error occurred during login. Please try again." + ex.Message;
            return View(loginModel);
        }
    }
    
    public IActionResult Logout()
    {
        // clear the session
        HttpContext.Session.Clear();
    
        // redirect to login page
        return RedirectToAction("Login", "Employees");
    }

    private RedirectToActionResult RedirectEmployee(EmployeeType employeeType)
    {
        switch (employeeType)
        {
            case EmployeeType.Waiter:
                return RedirectToAction("Overview", "Restaurant");
            case EmployeeType.Bartender:
                return RedirectToAction("Privacy", "Home");
            case EmployeeType.Cook:
                return RedirectToAction("Index", "Kitchen");
        }
        return RedirectToAction("Index", "Home");
    }
}
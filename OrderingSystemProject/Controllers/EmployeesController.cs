using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
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
        List<Employee> employees = _employeesService.GetAllEmployees();
        return View(employees);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        try
        {
            _employeesService.Create(employee);
            TempData["EmployeeOperationConfirmMessage"] = "Staff has been added!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = $"Exception occured: {e.Message}";
            return View(employee);
        }
    }
    
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = _employeesService.GetById((int)id);
        employee.Password = "";
        return View(employee);
    }

    [HttpPost]
    public IActionResult Edit(Employee employee)
    {
        try
        {
            _employeesService.Update(employee);

            TempData["EmployeeOperationConfirmMessage"] = "Staff has been updated!";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";

            return View(employee);
        }
    }

    [HttpPost]
    public IActionResult Activate(int employeeId)
    {
        try
        {
            _employeesService.Activate(employeeId);
        
            TempData["EmployeeOperationConfirmMessage"] = "Staff has been activated!";
        
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";

            return RedirectToAction(nameof(Index));
        }
        
    }
    
    [HttpPost]
    public IActionResult Deactivate(int employeeId)
    {
        try
        {
            _employeesService.Deactivate(employeeId);
        
            TempData["EmployeeOperationConfirmMessage"] = "Staff has been deactivated!";
        
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";

            return RedirectToAction(nameof(Index));
        }
        
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
                return RedirectEmployee((EmployeeType)employee.EmployeeType);
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
            Employee employee =
                _employeesService.GetEmployeeByLoginCredentials(loginModel.UserName, loginModel.Password);

            // if credentials are incorrect
            if (employee == null)
            {
                ViewData["Exception"] = "Invalid username or password";
                return View(loginModel);
            }
            else
            {
                // remember logged in employee
                string userJson = JsonSerializer.Serialize(employee);
                HttpContext.Session.SetString("LoggedInEmployee", userJson);

                // redirect Employee by Type
                return RedirectEmployee((EmployeeType)employee.EmployeeType);
            }
        }
        catch (Exception ex)
        {
            ViewData["Exception"] = "An error occurred during login. Please try again." + ex.Message;
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
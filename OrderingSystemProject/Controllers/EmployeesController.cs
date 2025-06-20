using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;

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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            List<Employee> employees = _employeesService.GetAllEmployees();
            return View(employees);
        }
        catch (Exception e)
        {
            TempData["Exception"] = $"Exception occured: {e.Message}";
            return View(new List<Employee>());
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        return View();
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            _employeesService.Create(employee);
            TempData["SuccessMessage"] = "Staff has been added!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(employee);
        }
    }


    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var employee = _employeesService.GetById((int)id);
            employee.Password = "";
            return View(employee);
        }
        catch (Exception e)
        {
            TempData["Exception"] = $"Exception occured: {e.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public IActionResult Edit(Employee employee)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            _employeesService.Update(employee);
            TempData["SuccessMessage"] = "Staff has been updated!";
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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            _employeesService.Activate(employeeId);
            TempData["SuccessMessage"] = "Staff has been activated!";
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
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            _employeesService.Deactivate(employeeId);
            TempData["SuccessMessage"] = "Staff has been deactivated!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public IActionResult Delete(int employeeId)
    {
        if (IsUnauthorisedUser())
        {
            return RedirectToAction("Login", "Employees");
        }

        try
        {
            _employeesService.Delete(employeeId);
            TempData["SuccessMessage"] = "Staff has been removed!";
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
        Employee? employee = HttpContext.Session.GetObject<Employee>("LoggedInEmployee");

        if (employee != null)
        {
            // if employee exists in session, redirect based on employee type
            return RedirectEmployee((EmployeeType)employee.EmployeeType);
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

            // check if Employee is active
            if (!employee.IsActive)
            {
                ViewData["Exception"] = "Your account has been deactivated. Please contact administrator.";
                return View(loginModel);
            }

            // remember logged in employee
            HttpContext.Session.SetObject("LoggedInEmployee", employee);

            // redirect Employee by Type
            return RedirectEmployee(employee.EmployeeType);
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
                return RedirectToAction("Index", "Bar");
            case EmployeeType.Cook:
                return RedirectToAction("Index", "Kitchen");
            case EmployeeType.Manager:
                return RedirectToAction("Index", "Employees");
        }

        return RedirectToAction("Index", "Home");
    }

    private bool IsUnauthorisedUser()
    {
        var userRole = Authorization.GetUserRole(HttpContext);
        return userRole != EmployeeType.Manager;
    }
}
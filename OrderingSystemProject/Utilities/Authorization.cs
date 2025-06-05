using System.Text.Json;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Utilities;

public class Authorization
{
    // get the role of logged in employee, returns null if not logged in
    public static EmployeeType? GetUserRole(HttpContext httpContext)
    {
        string? employeeJson = httpContext.Session.GetString("LoggedInEmployee");
        if (employeeJson != null)
        {
            Employee? employee = JsonSerializer.Deserialize<Employee>(employeeJson);
            return employee?.EmployeeType;
        }
        return null; // not logged in
    }
}
using System.Text.Json;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Utilities;

public class Authorization
{
    // get the role of logged in employee, returns null if not logged in
    public static EmployeeType? GetUserRole(HttpContext httpContext)
    {
        Employee? employee = httpContext.Session.GetObject<Employee>("LoggedInEmployee");
        if (employee != null)
        {
            return employee.EmployeeType;
        }
        return null; // not logged in
    }
}
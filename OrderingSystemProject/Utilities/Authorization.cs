using System.Text.Json;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Utilities;

public class Authorization
{
    public static bool IsUserLoggedIn(HttpContext httpContext)
    {
        string? employeeJson = httpContext.Session.GetString("LoggedInEmployee");
        return employeeJson != null;
    }
}
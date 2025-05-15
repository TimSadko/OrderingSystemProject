using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IEmployeesService
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeByLoginCredentials(string login, string password);
    
}
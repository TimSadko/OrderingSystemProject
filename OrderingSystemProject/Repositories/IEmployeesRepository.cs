using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IEmployeesRepository
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeByLogin(string userName);
}
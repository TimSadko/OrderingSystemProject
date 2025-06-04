using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IEmployeesService
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeByLoginCredentials(string userName, string password);
    
    void Create(Employee employee);
    
    Employee? GetById(int id);
    void Update(Employee employee);
    
    void Activate(int employeeId);
    
    void Deactivate(int employeeId);

}
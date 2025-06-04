using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IEmployeesRepository
{
    List<Employee> GetAllEmployees();
    Employee GetEmployeeByLogin(string userName);
    
    void Create(Employee employee);
    
    Employee? GetById(int id);
    
    void Update(Employee employee);
    
    void Activate(int employeeId);
    
    void Deactivate(int employeeId);
}
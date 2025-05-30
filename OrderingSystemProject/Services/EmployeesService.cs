using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Utilities;

namespace OrderingSystemProject.Services;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public List<Employee> GetAllEmployees()
    {
        return _employeesRepository.GetAllEmployees();
    }

    public Employee GetEmployeeByLoginCredentials(string userName, string password)
    {
        // get the employee by login
        Employee employee = _employeesRepository.GetEmployeeByLogin(userName);

        // if no employee found, return null
        if (employee == null)
        {
            return null;
        }

        // hash the entered password
        string hashedPassword = Hasher.GetHashString(password);

        // compare the stored hash with the hash of the entered password
        if (employee.Password == hashedPassword)
        {
            return employee;
        }

        // if passwords don't match
        return null;
    }

    public void Create(Employee employee)
    {
        _employeesRepository.Create(employee);
    }

    public Employee? GetById(int id)
    {
        return _employeesRepository.GetById(id);
    }

    public void Update(Employee employee)
    {
        _employeesRepository.Update(employee);
    }

    public void Activate(int employeeId)
    {
        _employeesRepository.Activate(employeeId);
    }

    public void Deactivate(int employeeId)
    {
        _employeesRepository.Deactivate(employeeId);
    }
}
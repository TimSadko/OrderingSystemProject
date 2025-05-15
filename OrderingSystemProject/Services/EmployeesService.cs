using System.Security.Cryptography;
using System.Text;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

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

    public Employee GetEmployeeByLoginCredentials(string login, string password)
    {
        try
        {
            // get the employee by login
            Employee employee =  _employeesRepository.GetEmployeeByLogin(login);
        
            // if no employee found, return null
            if (employee == null)
            {
                return null;
            }
            
            if (employee.Password == password)
            {
                return employee;
            }
            
            /*
            // check if password matches hash
            string hashedPassword = HashPassword(password);
            if (employee.Password != hashedPassword)
            {
                return null;
            }
            */
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during authentication: {ex.Message}");
            throw;
        }
    }
    
    /*
    // hash a password
        private string HashPassword(string password)
        {
            
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        
        */
}
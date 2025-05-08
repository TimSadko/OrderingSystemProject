using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public interface IEmployeeDB
    {
        List<Employee> GetAll();
        Employee? GetByLogin(string login);
        Employee? TryLogin(LoginModel model);
    }
}

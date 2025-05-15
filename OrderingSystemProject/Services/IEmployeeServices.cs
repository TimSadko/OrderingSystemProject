using OrderingSystemProject.Models;
using OrderingSystemProject.Other;

namespace OrderingSystemProject.Services
{
    public interface IEmployeeServices
    {
        Employee? TryLogin(LoginModel model);
    }
}

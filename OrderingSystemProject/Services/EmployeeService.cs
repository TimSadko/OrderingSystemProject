using OrderingSystemProject.Models;
using OrderingSystemProject.Other;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class EmployeeService : IEmployeeServices
    {
        private IEmployeeDB _rep;

        public EmployeeService(IEmployeeDB rep)
        {
            _rep = rep;

            CommonServices._employee_serv = this;
        }

        public Employee? TryLogin(LoginModel model)
        {
            Employee? emp = _rep.GetByLogin(model.Login);

            if (emp == null || emp.Password != Hasher.GetHashString(model.Password)) return null;

            return emp;
        }
    }
}

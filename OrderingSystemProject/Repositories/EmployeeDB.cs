using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Other;

namespace OrderingSystemProject.Repositories
{
    public class EmployeeDB : IEmployeeDB
    {
        private readonly string _connection_string;

        public EmployeeDB(DefaultConfiguration config) 
        {
            _connection_string = config.GetConnectionString();
        }

        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT login, employee_type, email, first_name, last_name, password From Employees ORDER BY last_name";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                Employee emp;

                while (reader.Read())
                {
                    emp = ReadEmployee(reader);
                    employees.Add(emp);
                }
                reader.Close();
            }

            return employees;
        }

        private Employee ReadEmployee(SqlDataReader reader)
        {
            return new Employee()
            {
                Login = (string)reader["login"],
                EmployeeType = (EMPLOYEE_TYPE)reader["employee_type"],
                Email = (string)reader["email"],
                FirstName = (string)reader["first_name"],
                LastName = (string)reader["last_name"],
                Password = (string)reader["password"]
            };
        }

        public Employee? GetByLogin(string login)
        {
            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT login, employee_type, email, first_name, last_name, password From Employees WHERE login = @login";

                SqlCommand com = new SqlCommand(query, conn);
                com.Parameters.AddWithValue("@login", login);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    return null;
                }

                reader.Read();
                Employee emp = ReadEmployee(reader);

                reader.Close();

                return emp;
            }
        }

        public Employee? TryLogin(LoginModel model)
        {
            Employee? emp = GetByLogin(model.Login);

            if (emp == null || emp.Password != Hasher.GetHashString(model.Password)) return null; 

            return emp;
        }
    }
}

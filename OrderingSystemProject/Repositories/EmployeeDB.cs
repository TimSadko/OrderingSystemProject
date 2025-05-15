using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Other;

namespace OrderingSystemProject.Repositories
{
    public class EmployeeDB : IEmployeeDB
    {
        private readonly string _connection_string;

        public EmployeeDB(IConfiguration config) 
        {
            _connection_string = config.GetConnectionString("OrderingDatabase");
        }

        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT EmployeeId, Login, Password, EmployeeType, FirstName, LastName, Email From Employees ORDER BY LastName";
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
            return new Employee((int)reader["EmployeeId"], (string)reader["Login"], (string)reader["Password"], (EMPLOYEE_TYPE)(int)reader["EmployeeType"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Email"]);
        }

        public Employee? GetByLogin(string login)
        {
            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT EmployeeId, Login, Password, EmployeeType, FirstName, LastName, Email From Employees WHERE Login = @Login";

                SqlCommand com = new SqlCommand(query, conn);
                com.Parameters.AddWithValue("@Login", login);

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

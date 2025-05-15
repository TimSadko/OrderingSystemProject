using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Other;

namespace OrderingSystemProject.Repositories
{
    public class DBEmployeesRepository : IEmployeesRepository
    {
        private readonly string _connection_string;

        public DBEmployeesRepository(IConfiguration config)
        {
            _connection_string = config.GetConnectionString("OrderingDatabase");
        }

        public List<Employee> GetAllEmployees()
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
            return new Employee((int)reader["EmployeeId"], (string)reader["Login"], (string)reader["Password"], (EmployeeType)(int)reader["EmployeeType"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Email"]);
        }

        public Employee GetEmployeeByLogin(string login)
        {
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                string query = "SELECT EmployeeId, Login, Password, EmployeeType, FirstName, LastName, Email FROM Employees WHERE Login = @login";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@login", login);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // check if employee exists
                if (reader.Read())
                {
                    Employee employee = ReadEmployee(reader);
                    reader.Close();
                    return employee;
                }

                reader.Close();
                return null; // Return null if no employee found
            }
        }
    }
}
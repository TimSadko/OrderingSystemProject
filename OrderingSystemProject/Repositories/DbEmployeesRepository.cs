using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public class DbEmployeesRepository : IEmployeesRepository
{
    private readonly string? _connectionString;

    public DbEmployeesRepository(IConfiguration configuration)
    {
        // get (database) connectionstring from appsettings
        _connectionString = configuration.GetConnectionString("OrderingDatabase");
    }

    public List<Employee> GetAllEmployees()
    {
        List<Employee> employees = new List<Employee>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT EmployeeId, UserName, Password, EmployeeType, FirstName, LastName, Email FROM Employees";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // ReadEmployee converts a record into an Employee object
                Employee employee = ReadEmployee(reader);
                employees.Add(employee);
            }
            reader.Close();
        }
        return employees;
    }


    public Employee GetEmployeeByLogin(string userName)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT EmployeeId, UserName, Password, EmployeeType, FirstName, LastName, Email FROM Employees WHERE UserName = @userName";
            
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userName", userName);
            
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
            return null; // return null if no employee found
        }
    }

	private Employee ReadEmployee(SqlDataReader reader)
	{
		return new Employee((int)reader["EmployeeId"], (string)reader["UserName"], (string)reader["Password"], (EmployeeType)(int)reader["EmployeeType"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Email"]);
	}
}
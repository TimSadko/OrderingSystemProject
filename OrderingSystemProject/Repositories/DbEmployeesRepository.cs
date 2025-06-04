using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Utilities;

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
            string query =
                "SELECT EmployeeId, UserName, Password, EmployeeType, FirstName, LastName, Email, IsActive FROM Employees";
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
            string query =
                "SELECT EmployeeId, UserName, Password, EmployeeType, FirstName, LastName, Email, IsActive FROM Employees WHERE UserName = @userName";

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

    public void Create(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query =
                "INSERT INTO Employees (UserName, Password, EmployeeType, FirstName, LastName, Email, IsActive)" +
                " VALUES (@UserName, @Password, @EmployeeType, @FirstName, @LastName, @Email, @IsActive); SELECT SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", employee.UserName);
            command.Parameters.AddWithValue("@Password", Hasher.GetHashString(employee.Password));
            command.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@IsActive", employee.IsActive);

            connection.Open();

            if (command.ExecuteNonQuery() == 0)
            {
                throw new Exception("Staff creation failed!");
            }
        }
    }

    public Employee? GetById(int id)
    {
        Employee? employee = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            string query = "SELECT EmployeeId, UserName, Password, EmployeeType, FirstName, LastName, Email, IsActive From Employees WHERE EmployeeId = @Id";
            SqlCommand com = new SqlCommand(query, conn);

            com.Parameters.AddWithValue("@Id", id);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                employee = ReadEmployee(reader);
            }

            reader.Close();
        }

        return employee;
    }

    public void Update(Employee employee)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Employees SET UserName = @UserName, Password = @Password, EmployeeType = @EmployeeType, FirstName = @FirstName, LastName = @LastName, Email = @Email, IsActive = @IsActive WHERE EmployeeId = @EmployeeId";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", employee.UserName);
            command.Parameters.AddWithValue("@Password", Hasher.GetHashString(employee.Password));
            command.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@IsActive", employee.IsActive);
            command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

            connection.Open();

            if (command.ExecuteNonQuery() == 0)
            {
                throw new Exception("Staff update failed!");
            }
        }
    }

    public void Activate(int employeeId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Employees SET IsActive = 1 WHERE EmployeeId = @EmployeeId";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            connection.Open();

            if (command.ExecuteNonQuery() == 0)
            {
                throw new Exception("Staff activation failed!");
            }
        }
    }

    public void Deactivate(int employeeId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Employees SET IsActive = 0 WHERE EmployeeId = @EmployeeId";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            connection.Open();

            if (command.ExecuteNonQuery() == 0)
            {
                throw new Exception("Staff deactivation failed!");
            }
        }
    }

    private Employee ReadEmployee(SqlDataReader reader)
    {
        return new Employee(
            (int)reader["EmployeeId"],
            (string)reader["UserName"],
            (string)reader["Password"],
            (EmployeeType)(int)reader["EmployeeType"],
            (string)reader["FirstName"],
            (string)reader["LastName"],
            (string)reader["Email"],
            (bool)reader["IsActive"]
        );
    }
}
using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public class DbTablesRepository : ITablesRepository
{
    private readonly string? _connectionString;

    public DbTablesRepository(IConfiguration configuration)
    {
        // get (database) connectionstring from appsettings
        _connectionString = configuration.GetConnectionString("OrderingDatabase");
    }

    public List<Table> GetAllTables()
    {
        List<Table> tables = new List<Table>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT TableId, TableNumber, Status FROM Tables";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // convert database record to Table object
                Table table = ReadTable(reader);
                tables.Add(table);
            }
            reader.Close();
        }
        return tables;
    }
    
    public Table? GetTableById(int tableId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT TableId, TableNumber, Status FROM Tables WHERE TableId = @tableId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableId", tableId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
                
            // check if the table exists
            if (reader.Read())
            {
                Table table = ReadTable(reader);
                reader.Close();
                return table;
            }
            reader.Close();
            return null; // return null if no tables found
        }
    }

	private Table ReadTable(SqlDataReader reader)
    {
      // retrieve data from fields and return new Table object
      return new Table((int)reader["TableId"], (TableStatus)(int)reader["Status"], (int)reader["TableNumber"]);
    }

    public void UpdateTableStatus(int tableId, TableStatus newTableStatus)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Tables SET Status = @status WHERE TableId = @tableId";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@tableId", tableId);
            command.Parameters.AddWithValue("@status", (int)newTableStatus);
            
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
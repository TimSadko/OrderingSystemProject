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
            string query = "SELECT TableId, Number, Status FROM Tables";
            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // ReadTable converts a record into a Table object
                Table table = ReadTable(reader);
                tables.Add(table);

                //var table = new Table
                //{
                //    TableId = reader.GetInt32(0),
                //    Number = reader.GetInt32(1),
                //    Status = reader.GetInt32(2) 
                //};
            }
            reader.Close();
        }
        return tables;
    }

    public Table GetTableByNumber(int tableNumber)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT TableId, Number, Status FROM Tables WHERE Number = @Number";
            //"SELECT TableId, Status FROM Tables WHERE TableId = @tableId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Number", tableNumber);
            //command.Parameters.AddWithValue("@tableId", tableId);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            // check if the table exists
            if (reader.Read())
            {
                Table table = ReadTable(reader);
                reader.Close();
                return table;

                //return new Table
                //{
                //    TableId = reader.GetInt32(0),
                //    Number = reader.GetInt32(1),
                //    Status = reader.GetInt32(2)
                //};
            }
            reader.Close();
            return null; // return null if no employee found
        }
    }

    private Table ReadTable(SqlDataReader reader)
    {
        // retrieve data from fields
        int tableId = (int)reader["TableId"];
        int tableNumber = (int)reader["Number"];
        int status = (int)reader["Status"];
        // return new Table object
        return new Table(tableId, status, tableNumber);
    }
}
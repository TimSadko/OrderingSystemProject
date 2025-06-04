using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public class DbTablesRepository : ITablesRepository
{
    private readonly string? _connectionString;
    private readonly IOrderItemsRepository _orderItemsRepository;
    
    public DbTablesRepository(IConfiguration configuration, IOrderItemsRepository orderItemsRepository)
    {
        // get (database) connectionstring from appsettings
        _connectionString = configuration.GetConnectionString("OrderingDatabase");
        _orderItemsRepository = orderItemsRepository;
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
    
    public Table GetTableByNumber(int tableId)
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
                return null; // return null if no employee found
            }
    }
    
    private Table ReadTable(SqlDataReader reader)
    {
        // retrieve data from fields
        int tableId = (int)reader["TableId"];
        int tableNumber = (int)reader["TableNumber"];
        int status = (int)reader["Status"];
        
        // return new Table object
        return new Table(tableId, tableNumber, status);
    }

    public List<Table> GetAllTablesWithOrders()
    {
        List<Table> tables = new List<Table>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // complex query with LEFT JOIN and subquery to get latest active order per table
            string query = "SELECT t.TableId, t.TableNumber, t.Status, o.OrderId, o.OrderStatus, o.OrderTime " +
                           "FROM Tables t LEFT JOIN Orders o ON t.TableId = o.TableId AND o.OrderStatus NOT IN (4) " +
                           "AND o.OrderTime = (SELECT MAX(o2.OrderTime) FROM Orders o2 WHERE o2.TableId = t.TableId " +
                           "AND o2.OrderStatus NOT IN (4)) ORDER BY t.TableNumber";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                // ReadTableInfo converts a record into a TableInfo object
                Table table = ReadTableWithOrder(reader);
                tables.Add(table);
            }
            reader.Close();
            
        }
        return tables;
    }

    private Table ReadTableWithOrder(SqlDataReader reader)
    {
        // retrieve data from fields
        int tableId = (int)reader["TableId"];
        int tableNumber = (int)reader["TableNumber"];
        int status = (int)reader["Status"];
        
        // check if there is an active order for this table (left join can return null)
        if (reader["OrderId"] != DBNull.Value)
        {
            // table has an active order - create Order object
            int orderId = (int)reader["OrderId"];
            OrderStatus orderStatus = (OrderStatus)(int)reader["OrderStatus"];
            DateTime orderTime = (DateTime)reader["OrderTime"];
            
            // create Order object
            Order activeOrder = new Order(orderId, tableId, orderStatus, orderTime);
            // load order items to enable FoodStatus and DrinkStatus computed properties
            activeOrder.Items = _orderItemsRepository.GetOrderItems(orderId);
        
           // return Table with active order
            return new Table(tableId, tableNumber, status, activeOrder);
        }
        else
        {
            // table has no active order
            return new Table(tableId, tableNumber, status, null);
        }
    }
}
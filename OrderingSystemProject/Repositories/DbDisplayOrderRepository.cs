using Microsoft.Data.SqlClient;

namespace OrderingSystemProject.Repositories;
using OrderingSystemProject.Models;

public class DbDisplayOrderRepository : IDisplayOrderRepository
{
    private readonly string _connection_string;

    public DbDisplayOrderRepository(IConfiguration config)
    {
        _connection_string = config.GetConnectionString("OrderingDatabase");
    }
    
    public DisplayOrderViewModel GetFullOrderSummary(int orderId)
       {
           List<OrderLineItemViewModel> orderLines = new List<OrderLineItemViewModel>();
           Order? order = null;
       
           using (SqlConnection connection = new SqlConnection(_connection_string))
           {
               var query = @"
                   SELECT 
                       o.OrderId,
                       o.OrderStatus,
                       o.OrderTime,
                       o.TableId,
                       
                       oi.OrderItemId,
                       oi.MenuItemId,
                       oi.Amount AS Quantity,
                       oi.Comment,
                       oi.ItemStatus,
                       
                       mi.Name,
                       mi.Price
       
                   FROM Orders o
                   JOIN OrderItems oi ON o.OrderId = oi.OrderId
                   JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
                   WHERE o.OrderId = @OrderId;
               ";
       
               SqlCommand command = new SqlCommand(query, connection);
               command.Parameters.AddWithValue("@OrderId", orderId);
               command.Connection.Open();
       
               SqlDataReader reader = command.ExecuteReader();
       
               while (reader.Read())
               {
                   var orderLine = new OrderLineItemViewModel(
                       (int)reader["OrderItemId"],
                       (int)reader["MenuItemId"],
                       (int)reader["Quantity"],
                       (string)reader["Comment"],
                       (string)reader["Name"],
                       (decimal)reader["Price"]
                   );
                   orderLines.Add(orderLine);
       
                   if (order == null)
                   {
                       int id = (int)reader["OrderId"];
                       OrderStatus status = (OrderStatus)(int)reader["OrderStatus"];
                       DateTime dateTime = (DateTime)reader["OrderTime"];
                       int tableId = (int)reader["TableId"];
       
                       order = new Order(id, status, dateTime, tableId);
                   }
               }
       
               reader.Close();
           }
           
           decimal orderTotal = orderLines.Sum(x => x.LineTotal);
       
           return new DisplayOrderViewModel
           {
               Order = order,
               OrderLines = orderLines,
               OrderTotal = orderTotal
           };
       }
}
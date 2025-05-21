using Microsoft.Data.SqlClient;

namespace OrderingSystemProject.Repositories;
using OrderingSystemProject.Models;

public class DbDisplayOrderRepository : IDisplayOrderRepository
{
    private const decimal VAT = 0.21m;
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
       
                       order = new Order(id, tableId, status, dateTime);
                   }
               }
       
               reader.Close();
           }
           
           decimal orderSubtotal = GetOrderSubtotal(orderLines);
           decimal orderTotal = GetOrderTotal(orderLines);
           decimal vat = GetVat(orderLines);
       
           return new DisplayOrderViewModel
           {
               Order = order,
               OrderLines = orderLines,
               OrderSubtotal = orderSubtotal,
               OrderTotal = orderTotal,
               Vat = vat
           };
       }

    private decimal GetOrderSubtotal(List<OrderLineItemViewModel> orderLines)
    {
        try
        {
            decimal orderTotal = orderLines.Sum(orderLine => orderLine.Quantity * orderLine.Price);
            return orderTotal;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private decimal GetVat(List<OrderLineItemViewModel> orderLines)
    {
        try
        {
            decimal orderSubtotal = GetOrderSubtotal(orderLines);
            decimal vat = orderSubtotal * VAT;
            return vat;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private decimal GetOrderTotal(List<OrderLineItemViewModel> orderLines)
    {
        try
        {
            decimal orderSubtotal = GetOrderSubtotal(orderLines);
            decimal vat = GetVat(orderLines);
            decimal orderTotal = orderSubtotal + vat;
            return orderTotal;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}
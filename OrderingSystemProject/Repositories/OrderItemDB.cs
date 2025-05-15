using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public class OrderItemDB : IOrderItemDB
    {
        private readonly string _connection_string;

        public OrderItemDB(DefaultConfiguration config)
        {
            _connection_string = config.GetConnectionString();
        }

        public List<OrderItem> GetAll()
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT Id, OrderId, ItemId, Amount, Comment From OrderItems ORDER BY OrderId";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                OrderItem itm;
                if (!reader.HasRows)
                {
                    return null;
                }
                
                while (reader.Read())
                {
                    itm = ReadItem(reader);
                    items.Add(itm);
                }
                reader.Close();
            }

            return items;
        }
        
        public List<OrderItem>? GetOrderItem(int orderId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT Id, OrderId, ItemId, Amount, Comment From OrderItems WHERE OrderId = @OrderId ORDER BY OrderId";
                SqlCommand com = new SqlCommand(query, conn);
                SqlCommand command = new SqlCommand(query, conn);
            
                command.Parameters.AddWithValue("@OrderId", orderId);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                OrderItem itm;
                if (!reader.HasRows)
                {
                    return null;
                }
                
                while (reader.Read())
                {
                    itm = ReadItem(reader);
                    orderItems.Add(itm);
                }
                reader.Close();
            }

            return orderItems;
        }

        private OrderItem ReadItem(SqlDataReader reader)
        {
            return new OrderItem((int)reader["Id"], (int)reader["OrderId"], (int)reader["ItemId"], (int)reader["Amount"], (string)reader["Comment"]);
        }
    }
}

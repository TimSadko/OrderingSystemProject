using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public class DbOrderItemsRepository : IOrderItemsRepository
    {
        private readonly string _connection_string;

        public DbOrderItemsRepository(IConfiguration config)
        {
            _connection_string = config.GetConnectionString("OrderingDatabase");
        }

        public List<OrderItem> GetAll()
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT OrderItemId, OrderId, MenuItemId, Amount, Comment, ItemStatus From OrderItems ORDER BY OrderId";
                
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                OrderItem itm;

                while (reader.Read())
                {
                    itm = ReadItem(reader);
                    FillInItem(itm);

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
                string query = "SELECT OrderItemId, OrderId, MenuItemId, Amount, Comment, ItemStatus From OrderItems WHERE OrderId = @OrderId ORDER BY OrderId";
                
                SqlCommand com = new SqlCommand(query, conn);

				com.Parameters.AddWithValue("@OrderId", orderId);

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
                    FillInItem(itm);

                    orderItems.Add(itm);
                }
                reader.Close();
            }

            return orderItems;
        }

        private OrderItem ReadItem(SqlDataReader reader)
        {
            return new OrderItem((int)reader["OrderItemId"], (int)reader["OrderId"], (int)reader["MenuItemId"], (int)reader["Amount"], (string)reader["Comment"], (OrderItemStatus)(int)reader["ItemStatus"]);
        }
        private void FillInItem(OrderItem item)
        {
            item.MenuItem = CommonRepository._menu_item_rep.GetById(item.MenuItemId);       
        }

        public List<OrderItem> GetOrderItems(int order_id)
		{
			List<OrderItem> orderItems = new List<OrderItem>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderItemId, OrderId, MenuItemId, Amount, Comment, ItemStatus From OrderItems WHERE OrderId = @order_id ORDER BY OrderId";
				SqlCommand com = new SqlCommand(query, conn);

				com.Parameters.AddWithValue("@order_id", order_id);

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

				OrderItem itm;

				while (reader.Read())
				{
					itm = ReadItem(reader);
                    FillInItem(itm);

                    orderItems.Add(itm);
				}
				reader.Close();
			}

			return orderItems;
		}

        public void AddItem(OrderItem orderItem)
        {
            using (var connection = new SqlConnection(_connection_string))
            {
                string query = "INSERT INTO OrderItems (OrderId, MenuItemId, Amount, Comment, ItemStatus) VALUES (@OrderId, @MenuItemId, @Amount, @Comment, @ItemStatus); SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
                command.Parameters.AddWithValue("@MenuItemId", orderItem.MenuItemId);
                command.Parameters.AddWithValue("@Amount", orderItem.Amount);
                command.Parameters.AddWithValue("@Comment", orderItem.Comment);
                command.Parameters.AddWithValue("@ItemStatus", orderItem.ItemStatus);

                connection.Open();

                if (command.ExecuteNonQuery() == 0)
                {
                    throw new Exception("failed!");
                }
            }
        }
    }
}

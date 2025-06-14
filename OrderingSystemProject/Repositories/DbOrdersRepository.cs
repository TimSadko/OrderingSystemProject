using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public class DbOrdersRepository : IOrdersRepository
    {
        private readonly string _connection_string;

        public DbOrdersRepository(IConfiguration config)
        {
            _connection_string = config.GetConnectionString("OrderingDatabase");
        }

        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders ORDER BY OrderTime";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                Order ord;

                while (reader.Read())
                {
                    ord = ReadOrder(reader);
                    FillInOrder(ord);
                    orders.Add(ord);
                }
                reader.Close();
            }

            return orders;
        }
        
        public Order? GetById(int id)
        {
            Order? order = null;

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE OrderId = @OrderId";
                SqlCommand com = new SqlCommand(query, conn);
                
                com.Parameters.AddWithValue("@OrderId", id);
                
                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    order = ReadOrder(reader);
                    FillInOrder(order);
                }
                reader.Close();
            }

            return order;
        }
        public Order? GetOrderByTable(int tableId)
        {
            Order? order = null;

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE TableId = @TableId AND OrderStatus = 3";
                SqlCommand com = new SqlCommand(query, conn);
                
                com.Parameters.AddWithValue("@TableId", tableId);
                
                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    order = ReadOrder(reader);
                    FillInOrder(order);
                }
                reader.Close();
            }

            return order;
        }
        
        //New stuff
        
        public List<OrderItem> GetItemsForOrder(int orderId)
{
    List<OrderItem> items = new List<OrderItem>();

    using (var conn = new SqlConnection(_connection_string))
    {
        conn.Open();
        var cmd = new SqlCommand(@"
            SELECT 
                oi.OrderItemId, oi.OrderId, oi.MenuItemId, oi.Amount, oi.Comment, oi.ItemStatus,
                mi.MenuItemId, mi.Name, mi.Price, mi.Card, mi.Category, mi.Stock, mi.IsActive
            FROM OrderItems oi
            JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
            WHERE oi.OrderId = @OrderId", conn);

        cmd.Parameters.AddWithValue("@OrderId", orderId);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var orderItem = new OrderItem
                {
                    Id = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                    MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                    Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                    Comment = reader.GetString(reader.GetOrdinal("Comment")),
                    ItemStatus = (OrderItemStatus)reader.GetInt32(reader.GetOrdinal("ItemStatus")),
                    MenuItem = new MenuItem
                    {
                        MenuItemId = reader.GetInt32(reader.GetOrdinal("MenuItemId")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        Card = (ItemCard)reader.GetInt32(reader.GetOrdinal("Card")),
                        Category = (ItemCategory)reader.GetInt32(reader.GetOrdinal("Category")),
                        Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    }
                };

                items.Add(orderItem);
            }
        }
    }

    return items;
}

        private Order ReadOrder(SqlDataReader reader)
        {
            return new Order((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
        }

		private KitchenOrder ReadKitchenOrder(SqlDataReader reader)
		{
			return new KitchenOrder((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
		}

		private void FillInOrder(Order order)
        {
            order.Items = CommonRepository._order_item_rep.GetOrderItems(order.OrderId); // Get order items of current iteration order
            order.Table = CommonRepository._tables_rep.GetTableById(order.TableId); // Get table of current iteration order
        }

		private void FillInKitchenOrder(KitchenOrder order)
		{
			order.SetItems(CommonRepository._order_item_rep.GetOrderItemsNoDrinks(order.OrderId)); // Get order items of current iteration order
			order.Table = CommonRepository._tables_rep.GetTableById(order.TableId); // Get table of current iteration order
		}

		public List<KitchenOrder> GetOrdersKitchen()
        {
			List<KitchenOrder> orders = new List<KitchenOrder>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE OrderStatus = 0 OR OrderStatus = 1 OR OrderStatus = 2 ORDER BY OrderTime DESC";
				SqlCommand com = new SqlCommand(query, conn);

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

				KitchenOrder ord;

				while (reader.Read())
				{
					ord = ReadKitchenOrder(reader);
					FillInKitchenOrder(ord);
					orders.Add(ord);
				}
				reader.Close();
			}

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].Items.Count == 0) 
                {
                    orders.RemoveAt(i);
                    i--;
                }
            }

			return orders;
		}

		public List<KitchenOrder> GetDoneOrdersKitchen()
        {
			List<KitchenOrder> orders = new List<KitchenOrder>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE OrderStatus = 3 OR OrderStatus = 4 ORDER BY OrderTime DESC";
				SqlCommand com = new SqlCommand(query, conn);

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

				KitchenOrder ord;

				while (reader.Read())
				{
					ord = ReadKitchenOrder(reader);
					FillInKitchenOrder(ord);
					orders.Add(ord);
				}
				reader.Close();
			}

			for (int i = 0; i < orders.Count; i++)
			{
				if (orders[i].Items.Count == 0)
				{
					orders.RemoveAt(i);
					i--;
				}
			}

			return orders;
		}

		public bool UpdateOrderStatus(int _order_id, OrderStatus _new_status)
        {
			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "UPDATE Orders SET OrderStatus = @_new_status WHERE OrderId = @_order_id";
				SqlCommand com = new SqlCommand(query, conn);

				com.Parameters.AddWithValue("@_order_id", _order_id);
				com.Parameters.AddWithValue("@_new_status", (int)_new_status);

				com.Connection.Open();

				int eff = com.ExecuteNonQuery();

				return eff > 0;
			}
		}
	}
}

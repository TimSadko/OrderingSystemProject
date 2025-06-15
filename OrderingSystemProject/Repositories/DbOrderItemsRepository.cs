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

		public OrderItem? GetOrederItemById(int order_item_id)
		{
            OrderItem? item;

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderItemId, OrderId, MenuItemId, Amount, Comment, ItemStatus From OrderItems WHERE OrderItemId = @order_item_id ORDER BY OrderId";

				SqlCommand com = new SqlCommand(query, conn);

                com.Parameters.AddWithValue("@order_item_id", order_item_id);

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

                if (!reader.HasRows) item = null;
                else
                {
                    reader.Read();
                    item = ReadItem(reader);
                }

				reader.Close();
			}

            return item;
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

		public List<OrderItem> GetOrderItemsNoDrinks(int order_id)
		{
			List<OrderItem> orderItems = new List<OrderItem>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderItemId, OrderId, MenuItemId, Amount, Comment, ItemStatus From OrderItems WHERE OrderId = @order_id AND MenuItemId NOT IN (SELECT MenuItemId FROM MenuItems WHERE [Card] = 2 OR [Card] = 3) ORDER BY MenuItemId";
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

		public List<OrderItem> GetOrderItemsDrinksOnly(int order_id)
		{
			List<OrderItem> orderItems = new List<OrderItem>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderItemId, OrderId, MenuItemId, Amount, Comment, ItemStatus From OrderItems WHERE OrderId = @order_id AND MenuItemId IN (SELECT MenuItemId FROM MenuItems WHERE [Card] = 2 OR [Card] = 3) ORDER BY MenuItemId";
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

		public bool UpdateOrderItemStatus(int _order_item_id, OrderItemStatus _new_status)
        {
			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "UPDATE OrderItems SET ItemStatus = @_new_status WHERE OrderItemId = @_order_item_id";
				SqlCommand com = new SqlCommand(query, conn);

				com.Parameters.AddWithValue("@_order_item_id", _order_item_id);
				com.Parameters.AddWithValue("@_new_status", (int)_new_status);			

				com.Connection.Open();

				int eff = com.ExecuteNonQuery();

                return eff > 0;
			}          
		}
	}
}

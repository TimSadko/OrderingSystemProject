﻿using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Bar;
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
                // Select all orders sort by time
                string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders ORDER BY OrderTime";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                Order ord;

                while (reader.Read())
                {
                    // Parse order info from db
                    ord = ReadOrder(reader);

                    // Edit order with add data
                    FillInOrder(ord);
                    orders.Add(ord);
                }
                reader.Close();
            }

            return orders;
        }
        public void Add(Order order)
        {
            using (var connection = new SqlConnection(_connection_string))
            {
                // Insert new order into the Orders and retrieve OrderId
                string query =
                    "INSERT INTO Orders (TableId, OrderStatus, OrderTime) VALUES (@TableId, @OrderStatus, @OrderTime); SELECT SCOPE_IDENTITY()";//auto-generated pk
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TableId", order.TableId);
                command.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                command.Parameters.AddWithValue("@OrderTime", order.OrderTime);

                connection.Open();

                // Execute the query and store OrderId
                object result = command.ExecuteScalar();
                order.OrderId = Convert.ToInt32(result);
            }
        }
        public Order? GetById(int id)
        {
            Order? order = null;

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                // fetch single order
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

        private Order ReadOrder(SqlDataReader reader)
        {
            return new Order((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
        }

		private KitchenOrder ReadKitchenOrder(SqlDataReader reader)
		{
			return new KitchenOrder((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
		}

		private BarOrder ReadBarOrder(SqlDataReader reader)
		{
			return new BarOrder((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
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

		private void FillInBarOrder(BarOrder order)
		{
			order.SetItems(CommonRepository._order_item_rep.GetOrderItemsDrinksOnly(order.OrderId)); // Get order items of current iteration order
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
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE (OrderStatus = 3 OR OrderStatus = 4) AND OrderTime BETWEEN @TimeYes AND @TimeNow ORDER BY OrderTime DESC";
				SqlCommand com = new SqlCommand(query, conn);

				com.Parameters.AddWithValue("@TimeNow", DateTime.Now);
				com.Parameters.AddWithValue("@TimeYes", DateTime.Now.AddDays(-1));

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

		public List<BarOrder> GetOrdersBar()
		{
			List<BarOrder> orders = new List<BarOrder>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE OrderStatus = 0 OR OrderStatus = 1 OR OrderStatus = 2 ORDER BY OrderTime DESC";
				SqlCommand com = new SqlCommand(query, conn);

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

				BarOrder ord;

				while (reader.Read())
				{
					ord = ReadBarOrder(reader);
					FillInBarOrder(ord);
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

		public List<BarOrder> GetDoneOrdersBar()
		{
			List<BarOrder> orders = new List<BarOrder>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE (OrderStatus = 3 OR OrderStatus = 4) AND OrderTime BETWEEN @TimeYes AND @TimeNow ORDER BY OrderTime DESC";
				SqlCommand com = new SqlCommand(query, conn);

				com.Parameters.AddWithValue("@TimeNow", DateTime.Now);
				com.Parameters.AddWithValue("@TimeYes", DateTime.Now.AddDays(-1));

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

				BarOrder ord;

				while (reader.Read())
				{
					ord = ReadBarOrder(reader);
					FillInBarOrder(ord);
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
		
		public List<Order> GetActiveOrders()
		{
			List<Order> orders = new List<Order>();
    
			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				// all orders except completed (4)
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime FROM Orders WHERE OrderStatus < 4 ORDER BY OrderTime";
        
				SqlCommand command = new SqlCommand(query, conn);
				command.Connection.Open();
				SqlDataReader reader = command.ExecuteReader();
        
				while (reader.Read())
				{
					Order order = ReadOrder(reader);
					FillInOrder(order);
					orders.Add(order);
				}
				reader.Close();
			}
			return orders;
		}
		
		public List<Order> GetActiveOrdersByTable(int tableId)
		{
			List<Order> orders = new List<Order>();
			
			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime FROM Orders WHERE OrderStatus < 3 AND TableId = @tableId";
				SqlCommand command = new SqlCommand(query, conn);
				command.Parameters.AddWithValue("@tableId", tableId);
				
				command.Connection.Open();
				SqlDataReader reader = command.ExecuteReader();
       
				while (reader.Read())
				{
					Order order = ReadOrder(reader);
					FillInOrder(order);
					orders.Add(order);
				}
				reader.Close();
			}
			return orders;
		}
	}
}

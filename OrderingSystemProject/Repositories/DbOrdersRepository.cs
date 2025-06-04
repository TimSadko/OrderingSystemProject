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
                string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders";
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
                string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders ORDER BY TableId";
                SqlCommand com = new SqlCommand(query, conn);

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

        private Order ReadOrder(SqlDataReader reader)
        {
            return new Order((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
        }

        private void FillInOrder(Order order)
        {
            order.Items = CommonRepository._order_item_rep.GetOrderItems(order.OrderId); // Get order items of current iteration order         
        }

        public List<Order> GetOrdersKitchen()
        {
            return GetAll();
        }

        //public void Add(Order order)
        //{
        //    using (var connection = new SqlConnection(_connection_string))
        //    {
        //        string query = @"INSERT INTO Orders (TableId, OrderStatus, OrderTime)
        //               VALUES (@TableId, @OrderStatus, @OrderTime);
        //               SELECT CAST(SCOPE_IDENTITY() as int);";
        //        SqlCommand command = new SqlCommand(query, connection);

        //        command.Parameters.AddWithValue("@TableId", order.TableId);
        //        command.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
        //        command.Parameters.AddWithValue("@OrderTime", order.OrderTime);

        //        connection.Open();

        //        if (command.ExecuteNonQuery() == 0)
        //        {
        //            throw new Exception("Menu item creation failed!");
        //        }
        //    }
        //}

        public void Add(Order order)
        {
            using (var connection = new SqlConnection(_connection_string))
            {
                string query = @"INSERT INTO Orders (TableId, OrderStatus, OrderTime)
                         VALUES (@TableId, @OrderStatus, @OrderTime);
                         SELECT CAST(SCOPE_IDENTITY() as int);";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TableId", order.TableId);
                command.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                command.Parameters.AddWithValue("@OrderTime", order.OrderTime);

                connection.Open();
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    order.OrderId = Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Order creation failed!");
                }
            }
        }

        public Order? GetByIdWithItems(int id)
        {
            var order = GetById(id);
            if (order == null)
                return null;

            order.Items = CommonRepository._order_item_rep.GetOrderItems(order.OrderId);

            return order;
        }

        public void Update(Order order)
        {
            using (var connection = new SqlConnection(_connection_string))
            {
                string query = @"UPDATE Orders 
                         SET TableId = @TableId, OrderStatus = @OrderStatus, OrderTime = @OrderTime
                         WHERE OrderId = @OrderId";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TableId", order.TableId);
                command.Parameters.AddWithValue("@OrderStatus", (int)order.OrderStatus);
                command.Parameters.AddWithValue("@OrderTime", order.OrderTime);
                command.Parameters.AddWithValue("@OrderId", order.OrderId);

                connection.Open();
                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    throw new Exception("Order update failed!");
                }
            }
        }

    }
}

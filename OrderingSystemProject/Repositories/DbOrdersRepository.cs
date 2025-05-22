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
    }
}

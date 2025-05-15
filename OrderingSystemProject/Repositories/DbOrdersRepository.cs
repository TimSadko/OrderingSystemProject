using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public class DbOrdersRepository : IOrdersRepository
    {
        private readonly string _connection_string;

        public DbOrdersRepository(IConfiguration config)
        {
            _connection_string = config.GetConnectionString("OrderingDatabase");
        }

        public Order? GetById(int id)
        {
            Order? order = null;

            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                string query = "SELECT OrderId, TableNumber, OrderStatus, OrderTime From Orders ORDER BY TableNumber";
                SqlCommand com = new SqlCommand(query, connection);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    order = ReadOrder(reader);
                }
                reader.Close();
            }

            return order;
        }

        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT OrderId, TableNumber, OrderStatus, OrderTime From Orders ORDER BY TableNumber";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                Order ord;

                while (reader.Read())
                {
                    ord = ReadOrder(reader);
                    orders.Add(ord);
                }
                reader.Close();
            }

            return orders;
        }

        private Order ReadOrder(SqlDataReader reader)
        {
            return new Order((int)reader["OrderId"], (int)reader["TableNumber"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
        }
    }
}

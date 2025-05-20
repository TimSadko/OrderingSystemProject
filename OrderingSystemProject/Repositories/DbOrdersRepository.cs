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
                SqlCommand com = new SqlCommand(query, connection);

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
                }
                reader.Close();
            }

            return order;
        }

        private Order ReadOrder(SqlDataReader reader)
        {
            return new Order((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
        }

		public List<KOrder> GetOrdersKitchen() // Get list of non-completed orders. (for kitchen or bar)
		{
			List<KOrder> orders = new List<KOrder>();

			using (SqlConnection conn = new SqlConnection(_connection_string))
			{
				string query = "SELECT OrderId, TableId, OrderStatus, OrderTime From Orders WHERE OrderStatus = 0 OR OrderStatus = 1 ORDER BY TableId"; 
				SqlCommand com = new SqlCommand(query, conn);

				com.Connection.Open();
				SqlDataReader reader = com.ExecuteReader();

				KOrder ord;

				while (reader.Read())
				{
					ord = ReadKOrder(reader);
					orders.Add(ord);
				}
				reader.Close();
			}

			return orders;
		}

		private KOrder ReadKOrder(SqlDataReader reader)
		{
			return new KOrder((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
		}
	}
}

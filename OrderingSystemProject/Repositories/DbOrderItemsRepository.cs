using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

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
                string query = "SELECT Id, OrderId, ItemId, Amount, Comment From OrderItems ORDER BY OrderId";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                OrderItem itm;

                while (reader.Read())
                {
                    itm = ReadItem(reader);
                    items.Add(itm);
                }
                reader.Close();
            }

            return items;
        }

        private OrderItem ReadItem(SqlDataReader reader)
        {
            return new OrderItem((int)reader["Id"], (int)reader["OrderId"], (int)reader["ItemId"], (int)reader["Amount"], (string)reader["Comment"]);
        }
    }
}

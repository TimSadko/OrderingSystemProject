using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public class DbPaymentRepository : IPaymentRepository
{
    private readonly string _connection_string;

    public DbPaymentRepository(IConfiguration config)
    {
        _connection_string = config.GetConnectionString("OrderingDatabase");
    }
    public List<Order> GetAll()
    {
        List<Order> orders = new List<Order>();

        using (SqlConnection conn = new SqlConnection(_connection_string))
        {
            string query = "SELECT OrderId, OrderStatus, OrderTime, TableId From Orders ORDER BY TableNumber";
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
        return new Order((int)reader["OrderId"], (int)reader["TableId"], (OrderStatus)(int)reader["OrderStatus"], (DateTime)reader["OrderTime"]);
    }

    public void Pay(int orderId, int amount)
    {
        throw new NotImplementedException();
    }

    public void Add(Payment payment)
    {
        throw new NotImplementedException();
    }

    public bool IsPaymentExist(Payment payment)
    {
        throw new NotImplementedException();
    }

    public void Split(Payment payment)
    {
        throw new NotImplementedException();
    }
}
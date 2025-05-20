namespace OrderingSystemProject.Repositories;
using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

public class DbBillsRepository : IBillRepository
{
    private readonly string _connection_string;

    public DbBillsRepository(IConfiguration config)
    {
        _connection_string = config.GetConnectionString("OrderingDatabase");
    }
    public List<Bill> GetAll()
    {
        List<Bill> bills = new List<Bill>();

        using (SqlConnection conn = new SqlConnection(_connection_string))
        {
            string query = "SELECT OrderId, TableNumber, OrderStatus, OrderTime From Orders ORDER BY TableNumber";
            SqlCommand com = new SqlCommand(query, conn);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            Bill bill;

            while (reader.Read())
            {
                bill = ReadBill(reader);
                bills.Add(bill);
            }
            reader.Close();
        }
        
        return bills;
    }
    
    public Bill? GetById(int orderId)
    {
        Bill? bill = null;

        using (SqlConnection connection = new SqlConnection(_connection_string))
        {
            string query = "SELECT OrderId, TableNumber, OrderStatus, OrderTime From Orders ORDER BY TableNumber"; 
            SqlCommand com = new SqlCommand(query, connection);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.Read())
            {
                bill = ReadBill(reader);
            }
            reader.Close();
        }

        return bill;
    }
    
    private Bill ReadBill(SqlDataReader reader)
    {
        return new Bill();
    }
}
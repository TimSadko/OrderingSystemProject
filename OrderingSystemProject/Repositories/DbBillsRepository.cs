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
    public Bill? GetById(int id)
    {
        Bill? bill = null;

        using (SqlConnection conn = new SqlConnection(_connection_string))
        {
            string query = "SELECT BillId, OrderId, TotalAmount, Vat From Bills WHERE BillId = @id"; 
            SqlCommand com = new SqlCommand(query, conn);
            
            com.Parameters.AddWithValue("@id", id);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.Read())
            {
                bill = ReadBill(reader);
                FillInBill(bill);
            }
            reader.Close();
        }

        return bill;
    }
    public void InsertBill(Bill bill)
    {
        using (SqlConnection conn = new SqlConnection(_connection_string))
        {
            string query = "INSERT INTO Bills (OrderId, TotalAmount, Vat) OUTPUT INSERTED.BillId VALUES (@orderId, @total, @vat)";
            SqlCommand com = new SqlCommand(query, conn);

            com.Parameters.AddWithValue("@orderId", bill.OrderId);
            com.Parameters.AddWithValue("@total", bill.OrderTotal);
            com.Parameters.AddWithValue("@vat", bill.Vat);

            com.Connection.Open();
            bill.BillId = (int)com.ExecuteScalar();
        }
    }
    private Bill ReadBill(SqlDataReader reader)
    {
        return new Bill((int)reader["BillId"], (int)reader["OrderId"], (decimal)reader["TotalAmount"], (int)reader["Vat"]);
    }
    private void FillInBill(Bill bill)
    {
        bill.Order = CommonRepository._order_rep.GetById(bill.OrderId);
    }
}
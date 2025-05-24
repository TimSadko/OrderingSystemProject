using System.Runtime.InteropServices;
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
    public Payment? GetById(int id)
    {
        Payment? payment= null;

        using (SqlConnection conn = new SqlConnection(_connection_string))
        {
            string query = "SELECT PaymentId, BillId, TipAmount, PaymentType, PaymentAmount, Feedback From Payments WHERE PaymentId = @PaymentId"; 
            SqlCommand com = new SqlCommand(query, conn);
            
            com.Parameters.AddWithValue("@PaymentId", id);

            com.Connection.Open();
            SqlDataReader reader = com.ExecuteReader();

            if (reader.Read())
            {
                payment = ReadPayment(reader);
                FillInPayment(payment);
            }
            reader.Close();
        }

        return payment;
    }
    private Payment ReadPayment(SqlDataReader reader)
    {
        return new Payment((int)reader["PaymentId"], (int)reader["BillId"], (decimal)reader["TipAmount"],(int)reader["PaymentType"], (decimal)reader["PaymentAmount"], (string)reader["Feedback"]);
    }
    private void FillInPayment(Payment payment)
    {
        payment.Bill = CommonRepository._bill_rep.GetById(payment.BillId);
    }
}
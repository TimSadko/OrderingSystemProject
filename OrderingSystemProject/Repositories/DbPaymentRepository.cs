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
    public Payment InsertPayment(Payment payment)
    {
        using (var conn = new SqlConnection(_connection_string))
        {
            var query = @"INSERT INTO Payments (BillId, PaymentAmount, TipAmount, PaymentType, Feedback)
                      OUTPUT INSERTED.PaymentId
                      VALUES (@BillId, @PaymentAmount, @TipAmount, @PaymentType, @Feedback)";
            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@BillId", payment.BillId);
            cmd.Parameters.AddWithValue("@PaymentAmount", payment.PaymentAmount);
            cmd.Parameters.AddWithValue("@TipAmount", payment.TipAmount);
            cmd.Parameters.AddWithValue("@PaymentType", payment.PaymentType);
            cmd.Parameters.AddWithValue("@Feedback", payment.Feedback == null ? "" : payment.Feedback);

            conn.Open();
            payment.PaymentId = (int)cmd.ExecuteScalar();
        }

        return payment;
    }
    public List<Payment> GetPaymentsByBillId(int billId)
    {
        var payments = new List<Payment>();

        using (SqlConnection conn = new SqlConnection(_connection_string))
        {
            string query = @"SELECT PaymentId, BillId, TipAmount, PaymentType, PaymentAmount, Feedback 
                         FROM Payments 
                         WHERE BillId = @BillId";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@BillId", billId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var payment = ReadPayment(reader);
                        FillInPayment(payment);
                        payments.Add(payment);
                    }
                }
            }
        }

        return payments;
    }
    
    private Payment ReadPayment(SqlDataReader reader)
    {
        return new Payment((int)reader["PaymentId"], (int)reader["BillId"], (decimal)reader["TipAmount"],(PaymentType)(int)reader["PaymentType"], (decimal)reader["PaymentAmount"], (string)reader["Feedback"]);
    }
    private void FillInPayment(Payment payment)
    {
        payment.Bill = CommonRepository._bill_rep.GetById(payment.BillId);
    }
}
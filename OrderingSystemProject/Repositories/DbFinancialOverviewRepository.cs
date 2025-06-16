using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public class DbFinancialOverviewRepository : IFinancialOverviewRepository
{
    private readonly string? _connectionString;

    public DbFinancialOverviewRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderingDatabase");
    }

    public FinancialOverview? GetFinancialOverview(DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = @"
            SELECT
                SUM(CASE WHEN MI.Category = 1 THEN OI.Amount ELSE 0 END) AS TotalSalesDrinks,
                SUM(CASE WHEN MI.Category = 2 THEN OI.Amount ELSE 0 END) AS TotalSalesLunch,
                SUM(CASE WHEN MI.Category = 3 THEN OI.Amount ELSE 0 END) AS TotalSalesDinner,
                SUM(CASE WHEN MI.Category = 1 THEN OI.Amount * MI.Price ELSE 0 END) AS TotalIncomeDrinks,
                SUM(CASE WHEN MI.Category = 2 THEN OI.Amount * MI.Price ELSE 0 END) AS TotalIncomeLunch,
                SUM(CASE WHEN MI.Category = 3 THEN OI.Amount * MI.Price ELSE 0 END) AS TotalIncomeDinner,
                (
                    SELECT SUM(P.TipAmount)
                    FROM Payments P
                    INNER JOIN Bills B ON P.BillId = B.BillId
                    INNER JOIN Orders O2 ON B.OrderId = O2.OrderId
                    WHERE O2.OrderTime BETWEEN @StartDate AND @EndDate
                ) AS TotalTips
            FROM OrderItems OI
            INNER JOIN MenuItems MI ON OI.MenuItemId = MI.MenuItemId
            INNER JOIN Orders O ON OI.OrderId = O.OrderId
            WHERE O.OrderTime BETWEEN @StartDate AND @EndDate;
           
        ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            FinancialOverview? financialOverview = null;
            if (reader.HasRows)
            {
                reader.Read();
                financialOverview = ReadFinancialOverview(reader);
            }

            reader.Close();
            return financialOverview;
        }
    }

    public FinancialOverview? GetFinancialOverview()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = @"
            SELECT
                SUM(CASE WHEN MI.Category = 1 THEN OI.Amount ELSE 0 END) AS TotalSalesDrinks,
                SUM(CASE WHEN MI.Category = 2 THEN OI.Amount ELSE 0 END) AS TotalSalesLunch,
                SUM(CASE WHEN MI.Category = 3 THEN OI.Amount ELSE 0 END) AS TotalSalesDinner,
                SUM(CASE WHEN MI.Category = 1 THEN OI.Amount * MI.Price ELSE 0 END) AS TotalIncomeDrinks,
                SUM(CASE WHEN MI.Category = 2 THEN OI.Amount * MI.Price ELSE 0 END) AS TotalIncomeLunch,
                SUM(CASE WHEN MI.Category = 3 THEN OI.Amount * MI.Price ELSE 0 END) AS TotalIncomeDinner,
                (
                    SELECT SUM(P.TipAmount)
                    FROM Payments P
                    INNER JOIN Bills B ON P.BillId = B.BillId
                    INNER JOIN Orders O2 ON B.OrderId = O2.OrderId
                ) AS TotalTips
            FROM OrderItems OI
            INNER JOIN MenuItems MI ON OI.MenuItemId = MI.MenuItemId
            INNER JOIN Orders O ON OI.OrderId = O.OrderId
        ";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            FinancialOverview? financialOverview = null;
            if (reader.HasRows)
            {
                reader.Read();
                financialOverview = ReadFinancialOverview(reader);
            }

            reader.Close();
            return financialOverview;
        }
    }

    private FinancialOverview? ReadFinancialOverview(SqlDataReader reader)
    {
        try
        {
            return new FinancialOverview
            (
                (int)reader["TotalSalesDrinks"],
                (int)reader["TotalSalesLunch"],
                (int)reader["TotalSalesDinner"],
                (decimal)reader["TotalIncomeDrinks"],
                (decimal)reader["TotalIncomeLunch"],
                (decimal)reader["TotalIncomeDinner"],
                (decimal)reader["TotalTips"]
            );
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
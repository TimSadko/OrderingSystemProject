namespace OrderingSystemProject.Models;

public class FinancialOverview
{
    public int TotalSalesDrinks { get; set; }
    public int TotalSalesLunch { get; set; }
    public int TotalSalesDinner { get; set; }
    public decimal TotalIncomeDrinks { get; set; }
    public decimal TotalIncomeLunch { get; set; }
    public decimal TotalIncomeDinner { get; set; }
    public decimal TotalTips { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalSales { get; set; }

    public FinancialOverview()
    {
    }

    public FinancialOverview(int totalSalesDrinks, int totalSalesLunch, int totalSalesDinner, decimal totalIncomeDrinks, decimal totalIncomeLunch, decimal totalIncomeDinner, decimal totalTips)
    {
        TotalSalesDrinks = totalSalesDrinks;
        TotalSalesLunch = totalSalesLunch;
        TotalSalesDinner = totalSalesDinner;
        TotalIncomeDrinks = totalIncomeDrinks;
        TotalIncomeLunch = totalIncomeLunch;
        TotalIncomeDinner = totalIncomeDinner;
        TotalTips = totalTips;
        TotalIncome = totalIncomeDrinks + totalIncomeLunch + totalIncomeDinner;
        TotalSales = totalSalesDrinks + totalSalesLunch + totalSalesDinner;
    }
}
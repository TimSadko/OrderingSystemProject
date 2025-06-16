using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IFinancialOverviewRepository
{
    FinancialOverview? GetFinancialOverview(DateTime startDate, DateTime endDate);
    FinancialOverview? GetFinancialOverview();
}
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IFinancialOverviewService
{
    FinancialOverview? GetFinancialOverview();
    FinancialOverview? GetFinancialOverviewForPeriod(DatePeriod? period);
    FinancialOverview? GetFinancialOverviewForBetweenDates(DateTime? startDate, DateTime? endDate);
}
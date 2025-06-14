using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using static OrderingSystemProject.Models.DatePeriod;

namespace OrderingSystemProject.Services;

public class FinancialOverviewService : IFinancialOverviewService
{
    private readonly IFinancialOverviewRepository _financialOverviewRepository;

    public FinancialOverviewService(IFinancialOverviewRepository financialOverviewRepository)
    {
        _financialOverviewRepository = financialOverviewRepository;
    }

    public FinancialOverview? GetFinancialOverview()
    {
        return _financialOverviewRepository.GetFinancialOverview();
    }

    public FinancialOverview? GetFinancialOverviewForPeriod(DatePeriod? period)
    {
        if (period == null)
        {
            throw new Exception("Period has not been selected!. Please select a period.");
        }

        DateTime? startDate = period switch
        {
            Year => DateTime.Now.AddYears(-1),
            Month => DateTime.Now.AddMonths(-1),
            Quarter => DateTime.Now.AddMonths(-3),
            _ => null,
        };

        var financialOverview = (startDate == null)
            ? _financialOverviewRepository.GetFinancialOverview()
            : _financialOverviewRepository.GetFinancialOverview((DateTime)startDate, DateTime.Now);

        if (financialOverview == null)
        {
            throw new Exception(
                "Financial overview is not available for the selected period! Please select a valid one.");
        }

        return financialOverview;
    }

    public FinancialOverview? GetFinancialOverviewForBetweenDates(DateTime? startDate, DateTime? endDate)
    {
        if (startDate == null)
        {
            throw new Exception("Start date has not been selected!. Please select a start date.");
        }

        if (endDate == null)
        {
            throw new Exception("End date has not been selected!. Please select an end date.");
        }

        var financialOverview =
            _financialOverviewRepository.GetFinancialOverview((DateTime)startDate, (DateTime)endDate);

        if (financialOverview == null)
        {
            throw new Exception(
                "Financial overview is not available for the selected dates! Please select another dates.");
        }

        return financialOverview;
    }
}
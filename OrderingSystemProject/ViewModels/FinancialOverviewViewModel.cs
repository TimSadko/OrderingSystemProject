using OrderingSystemProject.Models;

namespace OrderingSystemProject.ViewModels;

public class FinancialOverviewViewModel
{
    public DateFilterMode FilterMode { get; set; }
    public DatePeriod? SelectedPeriod { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public FinancialOverview? FinancialOverview { get; set; }


    public FinancialOverviewViewModel()
    {
    }

    public FinancialOverviewViewModel(DateFilterMode filterMode, DatePeriod? selectedPeriod, DateTime? startDate,
        DateTime? endDate, FinancialOverview? financialOverview)
    {
        FilterMode = filterMode;
        SelectedPeriod = selectedPeriod;
        StartDate = startDate;
        EndDate = endDate;
        FinancialOverview = financialOverview;
    }
}
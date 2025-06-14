using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Services;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Controllers;

public class FinancialOverviewController : Controller
{
    private readonly IFinancialOverviewService _financialOverviewService;

    public FinancialOverviewController(IFinancialOverviewService financialOverviewService)
    {
        _financialOverviewService = financialOverviewService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var financialOverviewViewModel = new FinancialOverviewViewModel
        {
            FilterMode = DateFilterMode.Standard
        };
        try
        {
            financialOverviewViewModel.FinancialOverview = _financialOverviewService.GetFinancialOverview();
            return View(financialOverviewViewModel);
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(financialOverviewViewModel);
        }
    }

    public IActionResult GetFinancialOverviewForPeriod(DatePeriod? selectedPeriod)
    {
        var financialOverviewViewModel = new FinancialOverviewViewModel
        {
            SelectedPeriod = selectedPeriod,
            FilterMode = DateFilterMode.Standard
        };
        try
        {
            financialOverviewViewModel.FinancialOverview =
                _financialOverviewService.GetFinancialOverviewForPeriod(selectedPeriod);

            TempData["SuccessMessage"] = GetSuccessMessageForPeriodSelection((DatePeriod)selectedPeriod!);

            return View(nameof(Index), financialOverviewViewModel);
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(nameof(Index), financialOverviewViewModel);
        }
    }

    public IActionResult GetFinancialOverviewForBetweenDates(DateTime? startDate, DateTime? endDate)
    {
        var financialOverviewViewModel = new FinancialOverviewViewModel
        {
            StartDate = startDate,
            EndDate = endDate,
            FilterMode = DateFilterMode.Custom
        };
        try
        {
            financialOverviewViewModel.FinancialOverview =
                _financialOverviewService.GetFinancialOverviewForBetweenDates(startDate, endDate);

            TempData["SuccessMessage"] = GetSuccessMessageForBetweenDates((DateTime)startDate!, (DateTime)endDate!);

            return View(nameof(Index), financialOverviewViewModel);
        }
        catch (Exception e)
        {
            ViewData["Exception"] = $"Exception occured: {e.Message}";
            return View(nameof(Index), financialOverviewViewModel);
        }
    }

    private static string GetSuccessMessageForPeriodSelection(DatePeriod selectedPeriod)
    {
        return selectedPeriod switch
        {
            DatePeriod.Month => $"Financial overview from {DateTime.Now.AddMonths(-1)} to today({DateTime.Now}).",
            DatePeriod.Quarter => $"Financial overview from {DateTime.Now.AddMonths(-3)} to today({DateTime.Now}).",
            DatePeriod.Year => $"Financial overview from {DateTime.Now.AddYears(-1)} to today({DateTime.Now}).",
            _ => $"Financial overview from beginning to today({DateTime.Now}). It shows all time financial data!",
        };
    }

    private static string GetSuccessMessageForBetweenDates(DateTime startDate, DateTime endDate)
    {
        return $"Financial overview for between {startDate} and {endDate}.";
    }
}
using System.ComponentModel.DataAnnotations;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.ViewModels;

public class MenuManagementViewModel
{
    public enum CardFilterType
    {
        [Display(Name = "Lunch")] LUNCH = 0,

        [Display(Name = "Dinner")] DINNER = 1,

        [Display(Name = "Drinks")] DRINKS = 2,
        
        [Display(Name = "Alcoholic Drinks")] ALCOHOLIC_DRINKS = 3,

        [Display(Name = "All")] ALL = -1
    }

    public enum CategoryFilterType
    {
        [Display(Name = "Starters")] STARTERS = 0,

        [Display(Name = "Mains")] MAINS = 1,
        
        [Display(Name = "Deserts")] DESERTS = 2,

        [Display(Name = "All")] ALL = -1
    }

    public List<MenuItem> MenuItems { get; set; }
    public CardFilterType CardFilter { get; set; }
    public CategoryFilterType CategoryFilter { get; set; }

    public MenuManagementViewModel()
    {
    }

    public MenuManagementViewModel(List<MenuItem> menuItems, CardFilterType cardFilter,
        CategoryFilterType categoryFilter)
    {
        MenuItems = menuItems;
        CardFilter = cardFilter;
        CategoryFilter = categoryFilter;
    }
}
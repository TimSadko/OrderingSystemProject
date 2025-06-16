using OrderingSystemProject.Models;

namespace OrderingSystemProject.ViewModels;

public class MenuManagementViewModel
{

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
using OrderingSystemProject.Models;
using System.Collections.Generic;
using static OrderingSystemProject.ViewModels.MenuManagementViewModel;

namespace OrderingSystemProject.ViewModels
{
    public class WaiterViewModel
    {
        public WaiterViewModel(CardFilterType cardFilter, CategoryFilterType categoryFilter, List<MenuItem> menuItems)
        {
            CardFilter = cardFilter;
            CategoryFilter = categoryFilter;
            MenuItems = menuItems;
        }

        public WaiterViewModel()
        {

        }

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public List<OrderItem> Cart { get; set; } = new List<OrderItem>();

        public int MenuItemId { get; set; }
        public Order Order { get; set; }
        public int TableNumber { get; set; }
        public Table Table { get; set; }

        public CardFilterType CardFilter { get; set; }
        public CategoryFilterType CategoryFilter { get; set; }
    }
}

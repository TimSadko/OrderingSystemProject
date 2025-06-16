using OrderingSystemProject.Models;
using System.Collections.Generic;

namespace OrderingSystemProject.ViewModels
{
    public class WaiterViewModel
    {
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public List<OrderItem> Cart { get; set; } = new List<OrderItem>();

        public int MenuItemId { get; set; }
        public Order Order { get; set; }
        public int TableNumber { get; set; }
        public Table Table { get; set; }
    }
}

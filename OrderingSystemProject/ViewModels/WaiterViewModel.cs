using OrderingSystemProject.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderingSystemProject.ViewModels
{
    public class WaiterViewModel
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        List<OrderItem> cart = new List<OrderItem>();

        public WaiterViewModel() { }

        public List<MenuItem> MenuItems { get { return menuItems; } set { menuItems = value; } }
        public List<OrderItem> Cart { get { return cart; } set { cart = value; } }

        public Table SelectedTable { get; set; }
    }
}

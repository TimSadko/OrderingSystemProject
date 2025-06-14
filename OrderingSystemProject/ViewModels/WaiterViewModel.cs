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

        //public List<MenuManagementViewModel> 

        public int MenuItemId{ get; set; }
        public Order order { get; set; }

        public int TableNumber { get; set; }

        public Table Table { get; set; }
    }
}


//public class WaiterViewModel
//{
//    public List<MenuItem> MenuItems { get; set; } = new();
//    public List<OrderItem> Cart { get; set; } = new();
//    public int MenuItemId { get; set; }
//    public Order? Order { get; set; }  // Nullable if not always set
//    public int TableNumber { get; set; }
//    public Table? Table { get; set; }
//}
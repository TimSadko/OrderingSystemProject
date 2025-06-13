using OrderingSystemProject.Models;

namespace OrderingSystemProject.ViewModels;

public class RestaurantOverviewViewModel
{
    // list of all tables in restaurant
    public List<Table> Tables { get; set; }
    // dictionary for active orders by table id
    public Dictionary<int, Order> ActiveOrdersByTableId { get; set; }
    
    public RestaurantOverviewViewModel(List<Table> tables, Dictionary<int, Order> activeOrders)
    {
        Tables = tables;
        ActiveOrdersByTableId = activeOrders;
    }
    
    // empty constructor
    public RestaurantOverviewViewModel()
    {
        Tables = new List<Table>();
        ActiveOrdersByTableId = new Dictionary<int, Order>();
    }
    
    // get order for specific table
    public Order? GetActiveOrderForTable(int tableId)
    {
        ActiveOrdersByTableId.TryGetValue(tableId, out Order? order);
        return order;
    }
}
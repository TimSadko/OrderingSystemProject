namespace OrderingSystemProject.Models;

public class OrderLineItemViewModel
{
    public int OrderItemId { get; set; }
    public int MenuItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Comment { get; set; }
    public decimal LineTotal => Price * Quantity;
    
    public OrderLineItemViewModel()
    {
        OrderItemId = 0;
        MenuItemId = 0;
        ItemName = "";
        Price = 0;
        Quantity = 0;
        Comment = "";
    }

    public OrderLineItemViewModel(int orderItemId, int menuItemId, int quantity, string comment, string itemName, decimal price)
    {
        OrderItemId = orderItemId;
        MenuItemId = menuItemId;
        ItemName = itemName;
        Price = price;
        Quantity = quantity;
        Comment = comment;
    }
}
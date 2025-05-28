namespace OrderingSystemProject.Models;

public class TableOrderInfo
{
    public int TableId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public ItemCard Card { get; set; }
    
    public TableOrderInfo()
    {
        // default constructor...
    }

    public TableOrderInfo(int tableId, OrderStatus orderStatus, ItemCard card)
    {
        TableId = tableId;
        OrderStatus = orderStatus;
        Card = card;
    }
}


namespace OrderingSystemProject.Models;

public enum TableStatus
{
    Available = 0,
    Occupied = 1,
}
public class Table
{
    public int TableId { get; set; }
    public int TableNumber { get; set; }
    public int Status { get; set; }
    
    public Table()
    {
        // default constructor...
    }
    
    public Table(int tableId, int tableNumber, int status)
    {
        TableId = tableId;
        TableNumber = tableNumber;
        Status = status;
    }

    // holds the active order for this table
    public Order? ActiveOrder { get; set; }
    
    // constructor to create table with order
    public Table(int tableId, int tableNumber, int status, Order? activeOrder)
    {
        TableId = tableId;
        TableNumber = tableNumber;
        Status = status;
        ActiveOrder = activeOrder;
    }

    public OrderItemStatus? FoodStatus
    {
        get
        {
            // check if table has an active order or order with items
            if (ActiveOrder == null || ActiveOrder.Items == null) return null;
            
            bool hasReady = false;
            bool hasPreparing = false;
            bool hasNewItem = false;
            bool hasServed = false;
            
            // loop through all food items
            foreach (OrderItem item in ActiveOrder.Items)
            {
                // check if this is food (LUNCH or DINNER)
                if (item.MenuItem != null && (item.MenuItem.Card == ItemCard.LUNCH || item.MenuItem.Card == ItemCard.DINNER))
                {
                    if ((int)item.ItemStatus == 2) hasReady = true;        // Ready item status
                    if ((int)item.ItemStatus == 1) hasPreparing = true;  // Preparing item status
                    if ((int)item.ItemStatus == 0) hasNewItem = true;    // NewItem item status
                    if ((int)item.ItemStatus == 3) hasServed = true;     // Served item status
                }
            }
            // return by priority: Ready -> Preparing -> NewItem -> Served
            if (hasReady) return (OrderItemStatus)2;
            if (hasPreparing) return (OrderItemStatus)1;
            if (hasNewItem) return (OrderItemStatus)0;
            if (hasServed) return (OrderItemStatus)3;
        
            return null;
        }
    }
    
    public OrderItemStatus? DrinkStatus 
    { 
        get 
        {
            // check if table has an active order or order with items
            if (ActiveOrder == null || ActiveOrder.Items == null) 
                return null;
        
            bool hasReady = false;
            bool hasPreparing = false;
            bool hasNewItem = false;
            bool hasServed = false;
        
            // loop through all drink items
            foreach (OrderItem item in ActiveOrder.Items)
            {
                // check if this is drink (DRINKS or ALCOHOLIC_DRINKS)
                if (item.MenuItem != null && (item.MenuItem.Card == ItemCard.DRINKS || item.MenuItem.Card == ItemCard.ALCOHOLIC_DRINKS))
                {
                    if ((int)item.ItemStatus == 2) hasReady = true;        // Ready item status
                    if ((int)item.ItemStatus == 1) hasPreparing = true;  // Preparing item status
                    if ((int)item.ItemStatus == 0) hasNewItem = true;    // NewItem item status
                    if ((int)item.ItemStatus == 3) hasServed = true;     // Served item status
                }
            }
            // return by priority: Ready -> Preparing -> NewItem -> Served
            if (hasReady) return (OrderItemStatus)2;
            if (hasPreparing) return (OrderItemStatus)1;
            if (hasNewItem) return (OrderItemStatus)0;
            if (hasServed) return (OrderItemStatus)3;
        
            return null;
        }
    }
}
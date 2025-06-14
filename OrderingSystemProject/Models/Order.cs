using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Models
{
    public enum OrderStatus 
    { 
        New = 0, Preparing = 1, ReadyForPickup = 2, Served = 3, Completed = 4
    }

    public class Order
    {
		protected int _order_id;
		protected int _table_id;
		protected OrderStatus _order_status;
		protected DateTime _order_time;

        protected List<OrderItem> _items = new List<OrderItem>();
		protected Table _table;

        public Order() { }


        public Order(int order_id, int table_id, OrderStatus order_status, DateTime order_time)
        {
            _order_id = order_id;    
            _order_status = order_status;
            _order_time = order_time;
            _table_id = table_id;
        }

        public int OrderId { get => _order_id; set => _order_id = value; }
        public int TableId { get => _table_id; set => _table_id = value; }
        public OrderStatus OrderStatus { get => _order_status; set => _order_status = value; }
        public DateTime OrderTime { get => _order_time; set => _order_time = value; }

        public List<OrderItem> Items { get => _items; set => _items = value; }
        public Table Table { get => _table; set => _table = value; }
        
        public OrderItemStatus? FoodStatus
        {
	        get
	        {
		        // check if order has items
		        if (_items == null || _items.Count == 0) return null;
        
		        bool hasReady = false;
		        bool hasPreparing = false;
		        bool hasNewItem = false;
		        bool hasServed = false;
        
		        // loop through all food items
		        foreach (OrderItem item in _items)
		        {
			        if (item.MenuItem != null && item.MenuItem.IsFood())
			        {
				        switch (item.ItemStatus)
				        {
					        case OrderItemStatus.Ready:
						        hasReady = true;
						        break;
					        case OrderItemStatus.Preparing:
						        hasPreparing = true;
						        break;
					        case OrderItemStatus.NewItem:
						        hasNewItem = true;
						        break;
					        case OrderItemStatus.Served:
						        hasServed = true;
						        break;
				        }
			        }
		        }
		        // return by priority 
		        if (hasReady) return OrderItemStatus.Ready;
		        if (hasPreparing) return OrderItemStatus.Preparing;
		        if (hasNewItem) return OrderItemStatus.NewItem;
		        if (hasServed) return OrderItemStatus.Served;
    
		        return null;
	        }
        }

        public OrderItemStatus? DrinkStatus
        {
	        get
	        {
		        // check if order has items
		        if (_items == null || _items.Count == 0) return null;
        
		        bool hasReady = false;
		        bool hasPreparing = false;
		        bool hasNewItem = false;
		        bool hasServed = false;
        
		        // loop through all food items
		        foreach (OrderItem item in _items)
		        {
			        if (item.MenuItem != null && item.MenuItem.IsDrink())
			        {
				        switch (item.ItemStatus)
				        {
					        case OrderItemStatus.Ready:
						        hasReady = true;
						        break;
					        case OrderItemStatus.Preparing:
						        hasPreparing = true;
						        break;
					        case OrderItemStatus.NewItem:
						        hasNewItem = true;
						        break;
					        case OrderItemStatus.Served:
						        hasServed = true;
						        break;
				        }
			        }
		        }
		        // return by priority 
		        if (hasReady) return OrderItemStatus.Ready;
		        if (hasPreparing) return OrderItemStatus.Preparing;
		        if (hasNewItem) return OrderItemStatus.NewItem;
		        if (hasServed) return OrderItemStatus.Served;
    
		        return null;
	        }
        }
	}
}

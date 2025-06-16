

namespace OrderingSystemProject.Models.Kitchen
{
	public class KitchenOrder : Order
	{
		private List<OrderItem> _items_starters = new List<OrderItem>();
		private List<OrderItem> _items_mains = new List<OrderItem>();
		private List<OrderItem> _items_deserts = new List<OrderItem>();

		public List<OrderItem> ItemStarters { get => _items_starters; set => _items_starters = value; }
		public List<OrderItem> ItemMains { get => _items_mains; set => _items_mains = value; }
		public List<OrderItem> ItemDeserts { get => _items_deserts; set => _items_deserts = value; }

		public KitchenOrder() : base() { }


		public KitchenOrder(int order_id, int table_id, OrderStatus order_status, DateTime order_time) : base(order_id, table_id, order_status, order_time) { }

		public KitchenOrder(Order order)
		{
			_order_id = order.OrderId;
			_table_id = order.TableId;
			_table = order.Table;
			_order_status = order.OrderStatus;
			_order_time = order.OrderTime;

			SetItems(order.Items);
		}

		public void SetItems(List<OrderItem> list)
		{
			_items = list;

            for (int i = 0; i < list.Count; i++)
            {
				if (list[i].MenuItem.Category == ItemCategory.STARTERS) _items_starters.Add(list[i]);
				else if (list[i].MenuItem.Category == ItemCategory.MAINS) _items_mains.Add(list[i]);
				else if (list[i].MenuItem.Category == ItemCategory.DESERTS) _items_deserts.Add(list[i]);
            }
        }

		public TimeSpan TimeSinceOrder { get => (DateTime.Now - _order_time); }

		public OrderStatus KitchenStatus
		{
			get
			{
				if (_order_status == OrderStatus.New) return OrderStatus.New;
				if (_order_status == OrderStatus.Served || _order_status == OrderStatus.Completed) return OrderStatus.Completed;

				int _new = 0, _prep = 0, _red = 0, _serv = 0;

				for (int i = 0; i < _items.Count; i++)
				{
					if (_items[i].MenuItem.Card == ItemCard.DRINKS) continue;

					if (_items[i].ItemStatus == OrderItemStatus.Preparing) _prep++;
					else if (_items[i].ItemStatus == OrderItemStatus.Ready) _red++;
					else if (_items[i].ItemStatus == OrderItemStatus.NewItem) _new++;
					else if (_items[i].ItemStatus == OrderItemStatus.Served) _serv++;
				}

				if(_serv == _items.Count) return OrderStatus.Served;

				if (_prep == 0 && _red == 0) return OrderStatus.New;
				else if (_new == 0 && _prep == 0) return OrderStatus.ReadyForPickup;

				return OrderStatus.Preparing;
			}
		}
	}
}

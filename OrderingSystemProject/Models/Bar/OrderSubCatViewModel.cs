using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Models.Bar
{
	public class OrderSubCatViewModel
	{
		public List<OrderItem> Subcat { get; set; }

		public BarOrder Order { get; set; }

		public ItemCard CatType { get; set; }
		public string CatName { get; set; }

		public OrderSubCatViewModel(List<OrderItem> subcat, BarOrder order, ItemCard catType, string catName)
		{
			Subcat = subcat;
			Order = order;
			CatType = catType;
			CatName = catName;
		}

		public OrderItemStatus CatStatus
		{
			get
			{
				if (Order.OrderStatus == OrderStatus.New) return OrderItemStatus.NewItem;

				int _new = 0, _prep = 0, _red = 0, _serv = 0;

				for (int i = 0; i < Subcat.Count; i++)
				{
					if (Subcat[i].ItemStatus == OrderItemStatus.Preparing) _prep++;
					else if (Subcat[i].ItemStatus == OrderItemStatus.Ready) _red++;
					else if (Subcat[i].ItemStatus == OrderItemStatus.NewItem) _new++;
					else if (Subcat[i].ItemStatus == OrderItemStatus.Served) _serv++;
				}

				if (_serv == Subcat.Count) return OrderItemStatus.Served;

				if (_prep == 0 && _red == 0) return OrderItemStatus.NewItem;
				else if (_new == 0 && _prep == 0) return OrderItemStatus.Ready;

				return OrderItemStatus.Preparing;
			}
		}
	}
}

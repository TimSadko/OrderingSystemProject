namespace OrderingSystemProject.Models.Kitchen
{
    public class OrderSubCatViewModel
    {
        public List<OrderItem> Subcat { get; set; }

        public KitchenOrder Order { get; set; }

        public ItemCategory CatType { get; set; }
        public string CatName { get; set; }

        public OrderSubCatViewModel(List<OrderItem> subcat, KitchenOrder order, ItemCategory catTypee, string catName)
        {
            Subcat = subcat;
			Order = order;
            CatType = catTypee;
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

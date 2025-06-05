namespace OrderingSystemProject.Models.Kitchen
{
    public class KitchenViewModel
    {
        private List<KitchenOrder>? _orders;
        private List<KitchenOrder>? _orders_ready;
        private DateTime _last_update;

		public KitchenViewModel(List<KitchenOrder> orders, List<KitchenOrder> orders_ready, DateTime last_update)
		{
			_orders = orders;
			_orders_ready = orders_ready;
			_last_update = last_update;
		}

		public List<KitchenOrder>? Orders { get => _orders; set => _orders = value; }
        public List<KitchenOrder>? ReadyOrders { get => _orders_ready; set => _orders_ready = value; }
        public DateTime LastUpdate { get => _last_update; set => _last_update = value; }
    }
}

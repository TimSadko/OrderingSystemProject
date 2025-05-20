namespace OrderingSystemProject.Models.Kitchen
{
	public class KOrder : Order
	{
		private List<KOrderItem> _items;

		public KOrder() { }

		public KOrder(int order_id, int table_number, OrderStatus order_status, DateTime order_time) : base(order_id, table_number, order_status, order_time)
		{
			_items = new List<KOrderItem>();
		}

		public List<KOrderItem> Items { get => _items; set => _items = value; }
	}
}

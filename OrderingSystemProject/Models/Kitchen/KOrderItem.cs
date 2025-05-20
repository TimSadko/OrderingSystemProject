namespace OrderingSystemProject.Models.Kitchen
{
	public class KOrderItem : OrderItem
	{
		private MenuItem _menu_item;

		public KOrderItem() { }

		public KOrderItem(int id, int order_id, int item_id, int amount, string comment, OrderItemStatus item_status) : base(id, order_id, item_id, amount, comment, item_status)
		{

		}

		public MenuItem MenuItem { get => _menu_item; set => _menu_item = value; }
	}
}

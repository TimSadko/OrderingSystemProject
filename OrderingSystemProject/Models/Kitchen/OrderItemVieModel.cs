namespace OrderingSystemProject.Models.Kitchen
{
	public class OrderItemVieModel
	{
		public OrderItem Item { get; set; }
		public KitchenOrder Order { get; set; }

		public OrderItemVieModel(OrderItem item, KitchenOrder order)
		{
			Item = item;
			Order = order;
		}
	}
}

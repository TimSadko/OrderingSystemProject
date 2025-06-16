using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Models.Bar
{
	public class OrderItemViewModel
	{
		public OrderItem Item { get; set; }
		public BarOrder Order { get; set; }

		public OrderItemViewModel(OrderItem item, BarOrder order)
		{
			Item = item;
			Order = order;
		}
	}
}

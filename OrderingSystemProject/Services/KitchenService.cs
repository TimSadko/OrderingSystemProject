using OrderingSystemProject.Models.Kitchen;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
	public class KitchenService : IKitchenServices
	{
		public KitchenService() 
		{

		}

		public List<KOrder> GetCookOrders()
		{
			List<KOrder> _orders = CommonRepository._order_rep.GetOrdersKitchen(); // Get list of orders for kitchen from order_repository

			List<KOrderItem>? buff_items; // Buffer that holds current iteretion order's order_items

			for (int i = 0; i < _orders.Count; i++)
			{
				buff_items = CommonRepository._order_item_rep.GetKOrdersKitchen(_orders[i].OrderId); // Get order items of current iteration order

				if (buff_items == null) continue; // If there are none go to next

				for (int j = 0; j < buff_items.Count; j++) // If there is any add them to the list
				{
					_orders[i].Items.Add(buff_items[j]); // Add current order's order_items to the order's list
					buff_items[j].MenuItem = CommonRepository._menu_item_rep.GetById(buff_items[j].MenuItemId); // Get menu item by ItemId and set current order item's menu item as it.
				}
			}

			return _orders;
		}
	}
}

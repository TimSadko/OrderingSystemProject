using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Services
{
	public interface IKitchenServices
	{
		List<KitchenOrder> GetCookOrders();

		List<KitchenOrder> GetCookOrdersReady(List<KitchenOrder> all);

		List<KitchenOrder> GetDoneCookOrders();	

		void TakeItem(int _order_id, int _item_id);

		void FinishItem(int _order_id, int _item_id);

		void ReturnItem(int _order_id, int _item_id);

		void TakeCat(int _order_id, int _cat);

		void FinishCat(int _order_id, int _cat);

		void ReturnCat(int _order_id, int _cat);

		void TakeOrder(int _order_id);

		void FinishOrder(int _order_id);

		void ReturnOrder(int _order_id);
	}
}

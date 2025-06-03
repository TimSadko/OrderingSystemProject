using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Services
{
	public interface IKitchenServices
	{
		List<KitchenOrder> GetCookOrders();

		List<KitchenOrder> GetDoneCookOrders();

		void TakeOrder(int _order_id, int _item_id);

		void FinishOrder(int _order_id, int _item_id);

		void TakeFullOrder(int _order_id);

		void FinishFullOrder(int _order_id);
	}
}

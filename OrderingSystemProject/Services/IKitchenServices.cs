using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services
{
	public interface IKitchenServices
	{
		List<Order> GetCookOrders();

		List<Order> GetDoneCookOrders();

		void TakeOrder(int _order_id, int _item_id);

		void FinishOrder(int _order_id, int _item_id);
	}
}

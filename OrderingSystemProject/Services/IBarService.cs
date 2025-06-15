using OrderingSystemProject.Models.Bar;

namespace OrderingSystemProject.Services
{
    public interface IBarService
	{
		List<BarOrder> GetBarOrders();

		List<BarOrder> GetBarOrdersReady(List<BarOrder> all);

		List<BarOrder> GetDoneBarOrders();

		void TakeItem(int _order_id, int _item_id);

		void FinishItem(int _order_id, int _item_id);

		void ReturnItem(int _order_id, int _item_id);

		void TakeOrder(int _order_id);

		void FinishOrder(int _order_id);

		void ReturnOrder(int _order_id);
	}
}

using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public interface IOrdersRepository
    {
		Order? GetById(int id);

		List<Order> GetAll();

		List<KitchenOrder> GetOrdersKitchen();

		List<KitchenOrder> GetDoneOrdersKitchen();

		bool UpdateOrderStatus(int _order_id, OrderStatus _new_status);
		
		List<Order> GetActiveOrders();
      
		List<Order> GetActiveOrdersByTable(int tableId);
      
		List<OrderItem> GetItemsForOrder(int orderId);
    }
}

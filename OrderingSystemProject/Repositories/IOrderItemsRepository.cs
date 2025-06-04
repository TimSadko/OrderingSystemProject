using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public interface IOrderItemsRepository
    {
        List<OrderItem> GetAll();
        OrderItem? GetOrederItemById(int order_item_id);

		List<OrderItem>? GetOrderItem(int orderId);

        List<OrderItem> GetOrderItems(int order_id);

        List<OrderItem> GetOrderItemsNoDrinks(int order_id);

        bool UpdateOrderItemStatus(int _order_item_id, OrderItemStatus _new_status);
      
        void AddItem(OrderItem orderItem);
    }
}

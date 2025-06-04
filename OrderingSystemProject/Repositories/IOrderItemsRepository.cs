using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public interface IOrderItemsRepository
    {
        List<OrderItem> GetAll();
        List<OrderItem>? GetOrderItem(int orderId);

        List<OrderItem> GetOrderItems(int order_id);

        void AddItem(OrderItem orderItem);
    }
}

using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public interface IOrderItemsRepository
    {
        List<OrderItem> GetAll();
        List<OrderItem>? GetOrderItem(int orderId);

		List<KOrderItem>? GetKOrdersKitchen(int order_id);
    }
}

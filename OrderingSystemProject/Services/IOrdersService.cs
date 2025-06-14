using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IOrdersService
{
    bool MarkOrderAsServed (int orderId, string itemType);
    List<Order> GetActiveOrders();                    
    List<Order> GetActiveOrdersByTable(int tableId);
}
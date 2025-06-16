using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services
{
    public interface IOrderService
    {
        Order CreateOrder(int tableId, List<OrderItem> items);
        bool MarkOrderAsServed(int orderId, string itemType);
        List<Order> GetActiveOrders();
        List<Order> GetActiveOrdersByTable(int tableId);
    }
}
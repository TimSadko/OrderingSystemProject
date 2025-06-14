using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services
{
    public interface IOrderService
    {
        Order CreateOrder(int tableId, List<OrderItem> items);
        //void SubmitOrder(int orderId);  
        //Order GetOrCreateActiveOrder(int tableId);

        //Order GetActiveOrder(int orderId);
        //void Save(Order order);

        //void AddItemToOrder(int orderId, int menuItemId, string comment = "");
        //void RemoveItemFromOrder(int orderId, int orderItemId);
        //void SubmitOrder(int orderId);
        //List<OrderItem> GetOrderItems(int orderId);
        //Order GetActiveOrderByTable(int tableId);
    }
}
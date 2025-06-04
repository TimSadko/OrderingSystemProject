using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services
{
    public interface IOrderService
    {
        Order StartOrderForTable(int tableId);
        Order GetActiveOrder(int orderId);
        void Save(Order order);
    }
}

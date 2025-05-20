using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IDisplayOrderRepository
{
    DisplayOrderViewModel GetFullOrderSummary(int orderId);
}
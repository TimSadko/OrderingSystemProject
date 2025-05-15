using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public interface IOrderItemsRepository
    {
        List<OrderItem> GetAll();
    }
}

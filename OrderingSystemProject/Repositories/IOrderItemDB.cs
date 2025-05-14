using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public interface IOrderItemDB
    {
        List<OrderItem> GetAll();
    }
}

using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public interface IOrdersRepository
    {
		Order? GetById(int id);

        List<Order> GetAll();

		List<Order> GetOrdersKitchen();

        void Add(Order order);

        Order GetByIdWithItems(int orderId);

        void Update(Order order);
    }
}

using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Repositories
{
    public interface IOrdersRepository
    {
		Order? GetById(int id);

		List<Order> GetAll();

		List<Order> GetOrdersKitchen();

		List<Order> GetDoneOrdersKitchen();
	}
}

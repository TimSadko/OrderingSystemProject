using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services
{
	public interface IKitchenServices
	{
		List<Order> GetCookOrders();
	}
}

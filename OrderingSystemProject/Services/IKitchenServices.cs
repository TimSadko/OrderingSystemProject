using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Services
{
	public interface IKitchenServices
	{
		List<KOrder> GetCookOrders();
	}
}

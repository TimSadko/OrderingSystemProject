using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
	public class KitchenService : IKitchenServices
	{
		public KitchenService() 
		{

		}

		public List<Order> GetCookOrders()
		{
			return CommonRepository._order_rep.GetOrdersKitchen();
		}
	}
}

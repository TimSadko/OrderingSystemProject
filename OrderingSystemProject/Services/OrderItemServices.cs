using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderItemServices : IOrderItemServices
    {
        private IOrderItemsRepository _rep;

        public OrderItemServices(IOrderItemsRepository rep)
        {
            _rep = rep;
        }
    }
}

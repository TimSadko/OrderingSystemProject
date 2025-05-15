using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderItemServices : IOrderItemServices
    {
        private IOrderItemDB _rep;

        public OrderItemServices(IOrderItemDB rep)
        {
            _rep = rep;

            CommonServices._order_item_serv = this;
        }
    }
}

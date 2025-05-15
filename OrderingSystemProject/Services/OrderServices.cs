using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderServices : IOrderServices
    {
        private IOrderDB _rep;

        public OrderServices(IOrderDB rep)
        {
            _rep = rep;

            CommonServices._order_serv = this;
        }
    }
}

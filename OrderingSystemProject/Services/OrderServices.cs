using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderServices : IOrderServices
    {
        private IOrdersRepository _rep;

        public OrderServices(IOrdersRepository rep)
        {
            _rep = rep;
        }
    }
}

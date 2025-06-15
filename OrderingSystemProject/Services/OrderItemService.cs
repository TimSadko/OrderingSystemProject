using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderItemService : IOrderItemService
    {
        private IOrderItemsRepository _orderItemsRepository;

        public OrderItemService(IOrderItemsRepository orderItemsRepository)
        {
            _orderItemsRepository = orderItemsRepository;
        }
    }
}

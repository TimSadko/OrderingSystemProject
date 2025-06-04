using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;

        public OrderService(IOrdersRepository ordersRepo, IOrderItemsRepository orderItemsRepo)
        {
            _ordersRepository = ordersRepo;
            _orderItemsRepository = orderItemsRepo;
        }

        public Order StartOrderForTable(int tableId)
        {
            var order = new Order
            {
                TableId = tableId,
                OrderStatus = 0, // Наприклад, статус "Активний"
                OrderTime = DateTime.Now,
                Items = new List<OrderItem>()
            };

            _ordersRepository.Add(order);

            return order;
        }

        public Order GetActiveOrder(int orderId)
        {
            return _ordersRepository.GetByIdWithItems(orderId); // повинен завантажувати разом з OrderItems
        }

        public void Save(Order order)
        {
            _ordersRepository.Update(order);
        }
    }

}

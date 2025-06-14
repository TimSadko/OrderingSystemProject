using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;

        public OrdersService(IOrdersRepository ordersRepository, IOrderItemsRepository orderItemsRepository)
        {
            _ordersRepository = ordersRepository;
            _orderItemsRepository = orderItemsRepository;
        }

        public List<Order> GetActiveOrders()
        {
            return _ordersRepository.GetActiveOrders();
        }

        public List<Order> GetActiveOrdersByTable(int tableId)
        {
            return _ordersRepository.GetActiveOrdersByTable(tableId);
        }
        
        public bool MarkOrderAsServed(int orderId, string itemType)
        {
            // get OrderItems of order
            List<OrderItem> orderItems = _orderItemsRepository.GetOrderItems(orderId);
            
            foreach (OrderItem item in orderItems)
            {
                if (((itemType == "food" && item.MenuItem.IsFood()) || 
                     (itemType == "drink" && item.MenuItem.IsDrink())) && item.ItemStatus == OrderItemStatus.Ready)
                    if (_orderItemsRepository.UpdateOrderItemStatus(item.Id, OrderItemStatus.Served))
                        return true;
            }
            return false;
        }
    }
}
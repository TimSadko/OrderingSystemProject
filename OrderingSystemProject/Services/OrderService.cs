using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly IOrderItemsRepository _order_item_rep;
        private readonly IMenuItemsRepository _menu_item_rep;

        public OrderService(IOrdersRepository ordersRepo, IOrderItemsRepository orderItemsRepo, IMenuItemsRepository menuItemsRep)
        {
            _ordersRepo = ordersRepo;
            _order_item_rep = orderItemsRepo;
            _menu_item_rep = menuItemsRep;
        }

        public Order CreateOrder(int tableId, List<OrderItem> items)
        {
            var order = new Order { TableId = tableId, OrderTime = DateTime.Now };
            _ordersRepo.Add(order);

            foreach (var item in items)
            {
                // Connect item to order
                item.OrderId = order.OrderId;
                _order_item_rep.Add(item);

                // Refreshed amount
                var menuItem = _menu_item_rep.GetById(item.MenuItemId);
                if (menuItem != null)
                {
                    menuItem.Stock -= item.Amount;

                    if (menuItem.Stock < 0)
                        menuItem.Stock = 0; // don't want to be less than 0

                    
                    _menu_item_rep.Update(menuItem);
                }
            }

            return order;
        }

        public List<Order> GetActiveOrders()
        {
            return _ordersRepo.GetActiveOrders();
        }

        public List<Order> GetActiveOrdersByTable(int tableId)
        {
            return _ordersRepo.GetActiveOrdersByTable(tableId);
        }

        public bool MarkOrderAsServed(int orderId, string itemType)
        {
            // get OrderItems of order
            List<OrderItem> orderItems = _order_item_rep.GetOrderItems(orderId);

            foreach (OrderItem item in orderItems)
            {
                if (((itemType == "food" && item.MenuItem.IsFood()) ||
                     (itemType == "drink" && item.MenuItem.IsDrink())) && item.ItemStatus == OrderItemStatus.Ready)
                    if (_order_item_rep.UpdateOrderItemStatus(item.Id, OrderItemStatus.Served))
                        return true;
            }
            return false;
        }
    }
}

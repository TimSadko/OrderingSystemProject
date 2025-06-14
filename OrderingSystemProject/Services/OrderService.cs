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

        //public Order CreateOrder(int tableId, List<OrderItem> items)
        //{
        //    var order = new Order { TableId = tableId, OrderTime = DateTime.Now };
        //    _ordersRepo.Add(order);

        //    foreach (var item in items)
        //    {
        //        item.OrderId = order.OrderId;
        //        _order_item_rep.AddItem(item);
        //    }

        //    return order;
        //}

        public Order CreateOrder(int tableId, List<OrderItem> items)
        {
            var order = new Order { TableId = tableId, OrderTime = DateTime.Now };
            _ordersRepo.Add(order);

            foreach (var item in items)
            {
                // Зв’язуємо позицію з новим замовленням
                item.OrderId = order.OrderId;
                _order_item_rep.AddItem(item);

                // Оновлюємо запас товару
                var menuItem = _menu_item_rep.GetById(item.MenuItemId);
                if (menuItem != null)
                {
                    menuItem.Stock -= item.Amount;

                    if (menuItem.Stock < 0)
                        menuItem.Stock = 0; // Запобігаємо від’ємному запасу

                    
                    _menu_item_rep.Update(menuItem);
                }
            }

            return order;
        }
    }
}

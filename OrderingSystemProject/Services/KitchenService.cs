using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;
using OrderingSystemProject.Repositories;
using System.Linq;

namespace OrderingSystemProject.Services
{
	public class KitchenService : IKitchenServices
	{
        private IOrdersRepository _order_rep;
        private IOrderItemsRepository _order_item_rep;

        public KitchenService(IOrdersRepository _order_rep, IOrderItemsRepository _order_item_rep)
        {
            this._order_rep = _order_rep;
            this._order_item_rep = _order_item_rep;
        }

        public List<KitchenOrder> GetCookOrders()
		{
			var list = _order_rep.GetOrdersKitchen(); // Get lists of orders for the repo

			return list; 
		}

		public List<KitchenOrder> GetCookOrdersReady(List<KitchenOrder> all)
		{
			var list = new List<KitchenOrder>();

            for (int i = 0; i < all.Count; i++)
            {
                if(all[i].KitchenStatus == OrderStatus.ReadyForPickup)
				{
					list.Add(all[i]);
					all.RemoveAt(i);
					i--;
				}
            }

            return list;
		}

		public List<KitchenOrder> GetDoneCookOrders()
		{
			return _order_rep.GetDoneOrdersKitchen();
		}

		public void TakeItem(int _order_id, int _item_id)
		{
			var item = _order_item_rep.GetOrederItemById(_item_id); // Get order item from rep

			if (item == null) throw new Exception("Invalid order item id"); // If it is null, throw exception
		

			if (item.ItemStatus != OrderItemStatus.NewItem) return; // If order item status is not new, return

			_order_item_rep.UpdateOrderItemStatus(_item_id, OrderItemStatus.Preparing); // Update the status of order item in db

			_order_rep.UpdateOrderStatus(_order_id, OrderStatus.Preparing); // Update the status of order in db			
		}

		public void FinishItem(int _order_id, int _item_id)
		{
			var item = _order_item_rep.GetOrederItemById(_item_id); // Get order item from rep

			if (item == null) throw new Exception("Invalid order item id"); // If it is null, throw exception

			if (item.ItemStatus != OrderItemStatus.Preparing) return; // If order item status is not new, return

			_order_item_rep.UpdateOrderItemStatus(_item_id, OrderItemStatus.Ready); // Update the status of order item in db

			var order = _order_rep.GetById(_order_id); // Get order from rep by id

			SetOrderStatusToReadyIfAllReady(order); // Set status of the order to served, if all of the items are served
		}

		public void ReturnItem(int _order_id, int _item_id)
		{
			var item = _order_item_rep.GetOrederItemById(_item_id); // Get order item from rep

			if (item == null) throw new Exception("Invalid order item id"); // If it is null, throw exception


			if (item.ItemStatus != OrderItemStatus.Preparing && item.ItemStatus != OrderItemStatus.Ready) return;

			_order_item_rep.UpdateOrderItemStatus(_item_id, OrderItemStatus.Preparing); // Update the status of order item in db

			var order = _order_rep.GetById(_order_id); // Get order from rep by id

			if(order.OrderStatus != OrderStatus.Preparing) _order_rep.UpdateOrderStatus(_order_id, OrderStatus.Preparing); // Update the status of order in db	
		}

		public void TakeOrder(int _order_id)
		{
			var order = new KitchenOrder(_order_rep.GetById(_order_id)); // Get order from rep by id

			if (order == null) throw new Exception("Invalid order id"); // if could not find order throw exception

			if (order.KitchenStatus != OrderStatus.New) return;

            for (int i = 0; i < order.Items.Count; i++) // Go throu list of order_item of the order
            {
				if (order.Items[i].MenuItem.Card == ItemCard.DRINKS || order.Items[i].MenuItem.Card == ItemCard.ALCOHOLIC_DRINKS) continue; // if item is drink, skip it

				if (order.Items[i].ItemStatus == OrderItemStatus.NewItem) // if current item status is new
				{
					_order_item_rep.UpdateOrderItemStatus(order.Items[i].Id, OrderItemStatus.Preparing); // Change status of the item in db
				}
            }

			_order_rep.UpdateOrderStatus(_order_id, OrderStatus.Preparing); // Update the status of order in db		
		}

		public void FinishOrder(int _order_id)
		{
			var order = new KitchenOrder(_order_rep.GetById(_order_id)); // Get order from rep by id

			if (order == null) throw new Exception("Invalid order id"); // if could not find order throw exception

			if (order.KitchenStatus != OrderStatus.Preparing) return;

			for (int i = 0; i < order.Items.Count; i++) // Go throu list of order_item of the order
			{
				if (order.Items[i].MenuItem.Card == ItemCard.DRINKS || order.Items[i].MenuItem.Card == ItemCard.ALCOHOLIC_DRINKS) continue; // if item is drink, skip it

				if (order.Items[i].ItemStatus == OrderItemStatus.NewItem || order.Items[i].ItemStatus == OrderItemStatus.Preparing) // if current item status is new or preparing
				{
					_order_item_rep.UpdateOrderItemStatus(order.Items[i].Id, OrderItemStatus.Ready); // Change status of the item in db
				}
			}

			SetOrderStatusToReadyIfAllReady(order); // Set status of the order to served, if all of the items are served	
		}

		public void ReturnOrder(int _order_id)
		{
			var order = new KitchenOrder(_order_rep.GetById(_order_id)); // Get order from rep by id

			if (order == null) throw new Exception("Invalid order id"); // if could not find order throw exception

			if (order.KitchenStatus != OrderStatus.ReadyForPickup) return;

			for (int i = 0; i < order.Items.Count; i++) // Go throu list of order_item of the order
			{
				if (order.Items[i].MenuItem.Card == ItemCard.DRINKS || order.Items[i].MenuItem.Card == ItemCard.ALCOHOLIC_DRINKS) continue; // if item is drink, skip it

				if (order.Items[i].ItemStatus == OrderItemStatus.Ready) // if current item status is new or preparing
				{
					_order_item_rep.UpdateOrderItemStatus(order.Items[i].Id, OrderItemStatus.Preparing); // Change status of the item in db
				}
			}

			_order_rep.UpdateOrderStatus(_order_id, OrderStatus.Preparing); // Update the status of order in db	
		}

		private void SetOrderStatusToReadyIfAllReady(Order order)
		{
			bool all_items_ready = true;

			for (int i = 0; i < order.Items.Count; i++) // Go throu list of order_items of the order, to determaine if all of them are ready
			{
				if (order.Items[i].ItemStatus == OrderItemStatus.NewItem || order.Items[i].ItemStatus == OrderItemStatus.Preparing)
				{
					all_items_ready = false;
					break;
				}
			}

			if (all_items_ready) _order_rep.UpdateOrderStatus(order.OrderId, OrderStatus.ReadyForPickup); // If all of them are ready, then update the status of order in db	
		}
	}
}

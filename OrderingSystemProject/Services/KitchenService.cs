using OrderingSystemProject.Models;
using OrderingSystemProject.Models.Kitchen;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
	public class KitchenService : IKitchenServices
	{
		public KitchenService() 
		{

		}

		public List<KitchenOrder> GetCookOrders()
		{
			var list = CommonRepository._order_rep.GetOrdersKitchen();

			list.Sort((o1, o2) =>
			{
				var _os1 = o1.KitchenStatus;
				var _os2 = o2.KitchenStatus;

				if(_os1 == OrderStatus.ReadyForPickup)
				{
					if (_os2 == OrderStatus.ReadyForPickup) return o1.OrderTime.CompareTo(o2.OrderTime);
					else return 1;
				}
				else if (_os2 == OrderStatus.ReadyForPickup)
				{
					return -1;
				}
				else return o1.OrderTime.CompareTo(o2.OrderTime);
			});

			return list;
		}

		public List<KitchenOrder> GetDoneCookOrders()
		{
			return CommonRepository._order_rep.GetDoneOrdersKitchen();
		}

		public void TakeOrder(int _order_id, int _item_id)
		{
			var item = CommonRepository._order_item_rep.GetOrederItemById(_item_id); // Get order item from rep

			if (item == null) throw new Exception("Invalid order item id"); // If it is null, throw exception
		

			if (item.ItemStatus != OrderItemStatus.NewItem) return; // If order item status is not new, return

			CommonRepository._order_item_rep.UpdateOrderItemStatus(_item_id, OrderItemStatus.Preparing); // Update the status of order item in db

			CommonRepository._order_rep.UpdateOrderStatus(_order_id, OrderStatus.Preparing); // Update the status of order in db			
		}

		public void FinishOrder(int _order_id, int _item_id)
		{
			var item = CommonRepository._order_item_rep.GetOrederItemById(_item_id); // Get order item from rep

			if (item == null) throw new Exception("Invalid order item id"); // If it is null, throw exception


			if (item.ItemStatus != OrderItemStatus.Preparing) return; // If order item status is not new, return

			CommonRepository._order_item_rep.UpdateOrderItemStatus(_item_id, OrderItemStatus.Ready); // Update the status of order item in db

			var order = CommonRepository._order_rep.GetById(_order_id); // Get order from rep by id

			SetOrderStatusToReadyIfAllReady(order); // Set status of the order to served, if all of the items are served
		}

		public void TakeFullOrder(int _order_id)
		{
			var order = new KitchenOrder(CommonRepository._order_rep.GetById(_order_id)); // Get order from rep by id

			if (order == null) throw new Exception("Invalid order id"); // if could not find order throw exception

			if (order.KitchenStatus != OrderStatus.New) return;

            for (int i = 0; i < order.Items.Count; i++) // Go throu list of order_item of the order
            {
				if (order.Items[i].MenuItem.Card == ItemCard.DRINKS) continue; // if item is drink, skip it

				if (order.Items[i].ItemStatus == OrderItemStatus.NewItem) // if current item status is new
				{
					CommonRepository._order_item_rep.UpdateOrderItemStatus(order.Items[i].Id, OrderItemStatus.Preparing); // Change status of the item in db
				}
            }

			CommonRepository._order_rep.UpdateOrderStatus(_order_id, OrderStatus.Preparing); // Update the status of order in db		
		}

		public void FinishFullOrder(int _order_id)
		{
			var order = new KitchenOrder(CommonRepository._order_rep.GetById(_order_id)); // Get order from rep by id

			if (order == null) throw new Exception("Invalid order id"); // if could not find order throw exception

			if (order.KitchenStatus != OrderStatus.Preparing) return;

			for (int i = 0; i < order.Items.Count; i++) // Go throu list of order_item of the order
			{
				if (order.Items[i].MenuItem.Card == ItemCard.DRINKS) continue; // if item is drink, skip it

				if (order.Items[i].ItemStatus == OrderItemStatus.NewItem || order.Items[i].ItemStatus == OrderItemStatus.Preparing) // if current item status is new or preparing
				{
					CommonRepository._order_item_rep.UpdateOrderItemStatus(order.Items[i].Id, OrderItemStatus.Ready); // Change status of the item in db
				}
			}

			SetOrderStatusToReadyIfAllReady(order); // Set status of the order to served, if all of the items are served	
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

			if (all_items_ready) CommonRepository._order_rep.UpdateOrderStatus(order.OrderId, OrderStatus.ReadyForPickup); // If all of them are ready, then update the status of order in db	
		}
	}
}

namespace OrderingSystemProject.Models
{
    public enum OrderItemStatus
    {

    }

    public class OrderItem
    {
        private int _id;
        private int _order_id;
        private int _menu_item_id;
        private int _amount;
        private string _comment;
        private OrderItemStatus _item_status;

		public OrderItem() { }


        public OrderItem(int id, int order_id, int menu_item_id, int amount, string comment, OrderItemStatus item_status)
        {
            _id = id;
            _order_id = order_id;
            _menu_item_id = menu_item_id;
			_amount = amount;
            _comment = comment;
            _item_status = item_status;
        }

		public int Id { get =>  _id; set => _id = value; }
        public int OrderId { get => _order_id; set => _order_id = value; }
        public int MenuItemId { get => _menu_item_id; set => _menu_item_id = value; }
        public int Amount { get => _amount; set => _amount = value; }
        public string Comment { get => _comment; set => _comment = value; }
        public OrderItemStatus ItemStatus { get => _item_status; set => _item_status = value; }
    }
}

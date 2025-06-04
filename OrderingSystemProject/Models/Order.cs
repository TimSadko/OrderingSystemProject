using OrderingSystemProject.Models.Kitchen;

namespace OrderingSystemProject.Models
{
    public enum OrderStatus 
    { 
        New = 0, Preparing = 1, ReadyForPickup = 2, Completed = 3
    }

    public class Order
    {
        private int _order_id;
        private int _table_id;
        private OrderStatus _order_status;
        private DateTime _order_time;

        private List<OrderItem> _items;


        public Order()
        {
            //_items = new List<OrderItem>();
        }

        public Order(int order_id, int table_id, OrderStatus order_status, DateTime order_time)
        {
            _order_id = order_id;
            _order_status = order_status;
            _order_time = order_time;
            _table_id = table_id;
            //_items = new List<OrderItem>(); 
        }

        public int OrderId { get => _order_id; set => _order_id = value; }
        public int TableId { get => _table_id; set => _table_id = value; }
        public OrderStatus OrderStatus { get => _order_status; set => _order_status = value; }
        public DateTime OrderTime { get => _order_time; set => _order_time = value; }

        public List<OrderItem> Items { get => _items; set => _items = value; }
    }
}

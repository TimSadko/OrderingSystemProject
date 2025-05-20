namespace OrderingSystemProject.Models
{
    public enum OrderStatus 
    { 

    }

    public class Order
    {
        private int _order_id;
        private OrderStatus _order_status;
        private DateTime _order_time;
        private int _table_id;

        public Order() { }

        public Order(int order_id, OrderStatus order_status, DateTime order_time, int table_id)
        {
            _order_id = order_id;
            _order_status = order_status;
            _order_time = order_time;
            _table_id = table_id;
        }

        public int OrderId { get => _order_id; set => _order_id = value; }
        
        public OrderStatus OrderStatus { get => _order_status; set => _order_status = value; }
        public DateTime OrderTime { get => _order_time; set => _order_time = value; }
        public int TableId { get => _table_id; set => _table_id = value; }
    }
}

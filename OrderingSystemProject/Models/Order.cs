namespace OrderingSystemProject.Models
{
    public enum OrderStatus 
    { 

    }

    public class Order
    {
        private int _order_id;
        private int _table_number;
        private OrderStatus _order_status;
        private DateTime _order_time;

        public Order() { }

        public Order(int order_id, int table_number, OrderStatus order_status, DateTime order_time)
        {
            _order_id = order_id;
            _table_number = table_number;
            _order_status = order_status;
            _order_time = order_time;
        }

        public int OrderId { get => _order_id; set => _order_id = value; }
        public int TableNumber { get => _table_number; set => _table_number = value; }
        public OrderStatus OrderStatus { get => _order_status; set => _order_status = value; }
        public DateTime OrderTime { get => _order_time; set => _order_time = value; }
    }
}

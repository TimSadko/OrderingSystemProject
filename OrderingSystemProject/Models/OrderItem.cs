namespace OrderingSystemProject.Models
{
    public class OrderItem
    {
        private int _id;
        private int _order_id;
        private int _item_id;
        private int _amount;
        private string _comment;

        public OrderItem() { }

        public OrderItem(int id, int order_id, int item_id, int amount, string comment)
        {
            _id = id;
            _order_id = order_id;
            _item_id = item_id;
            _amount = amount;
            _comment = comment;
        }

        public int Id { get =>  _id; set => _id = value; }
        public int OrderId { get => _order_id; set => _order_id = value; }
        public int ItemId { get => _item_id; set => _item_id = value; }
        public int Amount { get => _amount; set => _amount = value; }
        public string Comment { get => _comment; set => _comment = value; }
    }
}

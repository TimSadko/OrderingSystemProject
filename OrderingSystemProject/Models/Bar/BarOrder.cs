namespace OrderingSystemProject.Models.Bar
{
    public class BarOrder : Order
    {
        private List<OrderItem> _items_normal = new List<OrderItem>();
        private List<OrderItem> _items_alcoholic = new List<OrderItem>();

        public List<OrderItem> ItemsNormal { get => _items_normal; set => _items_normal = value; }
        public List<OrderItem> ItemsAlcoholic { get => _items_alcoholic; set => _items_alcoholic = value; }

        public BarOrder() : base() { }


        public BarOrder(int order_id, int table_id, OrderStatus order_status, DateTime order_time) : base(order_id, table_id, order_status, order_time) { }

        public BarOrder(Order order)
        {
            _order_id = order.OrderId;
            _table_id = order.TableId;
            _table = order.Table;
            _order_status = order.OrderStatus;
            _order_time = order.OrderTime;

            SetItems(order.Items);
        }

        public void SetItems(List<OrderItem> list)
        {
            _items = list;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].MenuItem.Card == ItemCard.DRINKS) _items_normal.Add(list[i]);
                else if (list[i].MenuItem.Card == ItemCard.ALCOHOLIC_DRINKS) _items_alcoholic.Add(list[i]);
            }
        }

        public TimeSpan TimeSinceOrder { get => DateTime.Now - _order_time; }

        public OrderStatus KitchenStatus
        {
            get
            {
                if (_order_status == OrderStatus.New) return OrderStatus.New;

                int _new = 0, _prep = 0, _red = 0, _serv = 0;

                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].MenuItem.Card != ItemCard.DRINKS && _items[i].MenuItem.Card != ItemCard.ALCOHOLIC_DRINKS) continue;

                    if (_items[i].ItemStatus == OrderItemStatus.Preparing) _prep++;
                    else if (_items[i].ItemStatus == OrderItemStatus.Ready) _red++;
                    else if (_items[i].ItemStatus == OrderItemStatus.NewItem) _new++;
                    else if (_items[i].ItemStatus == OrderItemStatus.Served) _serv++;
                }

                if (_serv == _items.Count) return OrderStatus.Served;

                if (_prep == 0 && _red == 0) return OrderStatus.New;
                else if (_new == 0 && _prep == 0) return OrderStatus.ReadyForPickup;

                return OrderStatus.Preparing;
            }
        }
    }
}

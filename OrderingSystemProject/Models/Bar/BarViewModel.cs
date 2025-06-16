namespace OrderingSystemProject.Models.Bar
{
    public class BarViewModel
    {
        private List<BarOrder>? _orders;
        private List<BarOrder>? _orders_ready;
        private DateTime _last_update;

        public BarViewModel(List<BarOrder>? orders, List<BarOrder>? orders_ready, DateTime last_update)
        {
            _orders = orders;
            _orders_ready = orders_ready;
            _last_update = last_update;
        }

        public List<BarOrder>? Orders { get => _orders; set => _orders = value; }
        public List<BarOrder>? ReadyOrders { get => _orders_ready; set => _orders_ready = value; }
        public DateTime LastUpdate { get => _last_update; set => _last_update = value; }
    }
}

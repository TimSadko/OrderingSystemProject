namespace OrderingSystemProject.Models.Kitchen
{
    public class KitchenViewModel
    {
        private List<KitchenOrder> _orders;
        private DateTime _last_update;

        public KitchenViewModel(List<KitchenOrder> orders, DateTime last_update)
        {
            _orders = orders;
            _last_update = last_update;
        }

        public List<KitchenOrder> Orders { get => _orders; set => _orders = value; }
        public DateTime LastUpdate { get => _last_update; set => _last_update = value; }
    }
}

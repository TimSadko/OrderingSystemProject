namespace OrderingSystemProject.Models
{
    public enum ItemCard
    {

    }

    public enum ItemCategory
    {
        
    }

    public class MenuItem
    {
        private int _item_id;
        private string _name;
        private decimal _price;
        private ItemCard _card;
        private ItemCategory _category;
        private int _stock;
        private bool _is_active;

        public MenuItem() { }

        public MenuItem(int item_id, string name, decimal price, ItemCard card, ItemCategory category, int stock, bool is_active)
        {
            _item_id = item_id;
            _name = name;
            _price = price;
            _card = card;
            _category = category;
            _stock = stock;
            _is_active = is_active;
        }

        public int ItemId { get => _item_id; set => _item_id = value; }
        public string Name { get => _name; set => _name = value; }
        public decimal Price { get => _price; set => _price = value; }
        public ItemCard Card { get => _card; set => _card = value; }
        public ItemCategory Category { get => _category; set => _category = value; }
        public int Stock { get => _stock; set => _stock = value; }
        public bool IsActive { get => _is_active; set => _is_active = value; }
    }
}

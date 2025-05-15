namespace OrderingSystemProject.Models
{
    public enum ITEM_CARD
    {

    }

    public enum ITEM_CATEGORY
    {
        
    }

    public class MenuItem
    {
        private int _item_id;
        private string _name;
        private decimal _price;
        private ITEM_CARD _card;
        private ITEM_CATEGORY _category;
        private int _stock;
        private bool _is_active;

        public MenuItem() { }

        public MenuItem(int item_id, string name, decimal price, ITEM_CARD card, ITEM_CATEGORY category, int stock, bool is_active)
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
        public ITEM_CARD Card { get => _card; set => _card = value; }
        public ITEM_CATEGORY Category { get => _category; set => _category = value; }
        public int Stock { get => _stock; set => _stock = value; }
        public bool IsActive { get => _is_active; set => _is_active = value; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace OrderingSystemProject.Models
{
    public enum ItemCard
    {
        [Display(Name = "Lunch")] LUNCH = 0,

        [Display(Name = "Dinner")] DINNER = 1,

        [Display(Name = "Drinks")] DRINKS = 2
    }

    public enum ItemCategory
    {
        [Display(Name = "Starters")] STARTERS = 0,

        [Display(Name = "Mains")] MAINS = 1,

        [Display(Name = "Entremets")] ENTREMENTS = 2,

        [Display(Name = "Deserts")] DESERTS = 3
    }

    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ItemCard Card { get; set; }
        public ItemCategory Category { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }

        public MenuItem()
        {
        }

        public MenuItem(int itemId, string name, decimal price, ItemCard card, ItemCategory category, int stock,
            bool isActive)
        {
            MenuItemId = itemId;
            Name = name;
            Price = price;
            Card = card;
            Category = category;
            Stock = stock;
            IsActive = isActive;
        }
    }
}
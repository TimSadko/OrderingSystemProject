using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderingSystemProject.Models
{
    public enum ItemCard
    {
        [Display(Name = "Drink")]
        DRINK = 0,
        [Display(Name = "Lunch")]
        LUNCH = 1,
        [Display(Name = "Dinner")]
        DINNER = 2
    }

    public enum ItemCategory
    {
        [Display(Name = "All")]
        ALL = 0,
        [Display(Name = "Starter")]
        STARTER = 1,
        [Display(Name = "Main")]
        MAIN = 2
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

        public MenuItem(int itemId, string name, decimal price, ItemCard card, ItemCategory category, int stock, bool isActive)
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
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public interface IMenuItemsRepository
    {
        List<MenuItem> GetAll();
        void Add(MenuItem menuItem);
        void Delete(MenuItem menuItem);
        MenuItem? GetById(int id);
        void Update(MenuItem menuItem);
        List<MenuItem> FilterByCategory(ItemCategory? category);
        List<MenuItem> FilterByCard(ItemCard? card);
        List<MenuItem> FilterByCategoryAndCard(ItemCategory? category, ItemCard? card);
        void Activate(int menuItemId);
        void Deactivate(int menuItemId);
    }
}

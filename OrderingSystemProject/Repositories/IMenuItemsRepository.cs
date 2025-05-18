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
    }
}

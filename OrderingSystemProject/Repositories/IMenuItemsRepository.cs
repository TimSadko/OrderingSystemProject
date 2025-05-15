using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public interface IMenuItemsRepository
    {
        List<MenuItem> GetAll();
        void Add(MenuItem menuItem);
    }
}

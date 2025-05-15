using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class MenuItemServices : IMenuItemServices
    {
        private IMenuItemsRepository _rep;

        public MenuItemServices(IMenuItemsRepository rep)
        {
            _rep = rep;
        }
    }
}

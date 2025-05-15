using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services
{
    public class MenuItemServices : IMenuItemServices
    {
        private IMenuItemDB _rep;

        public MenuItemServices(IMenuItemDB rep)
        {
            _rep = rep;

            CommonServices._menu_item_serv = this;
        }
    }
}

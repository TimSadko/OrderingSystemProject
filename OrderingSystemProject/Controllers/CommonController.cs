using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Controllers
{
    public class CommonController
    {
        public static IEmployeeDB _employee_rep;
        public static IOrderDB _order_rep;
        public static IMenuItemDB _menu_item_rep;
        public static IOrderItemDB _order_item_rep;
    }
}

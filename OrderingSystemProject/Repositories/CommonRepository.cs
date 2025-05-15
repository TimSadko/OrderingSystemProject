namespace OrderingSystemProject.Repositories
{
    public static class CommonRepository
    {
        public static IEmployeesRepository _employee_rep;
        public static IMenuItemsRepository _menu_item_rep;
        public static IOrdersRepository _order_rep;
        public static IOrderItemsRepository _order_item_rep;
        public static IBillRepository _bill_rep;
        public static IPaymentRepository _payment_rep;
    }
}

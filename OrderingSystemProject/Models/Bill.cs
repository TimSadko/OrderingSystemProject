namespace OrderingSystemProject.Models;

public class Bill
{
    public int BillId { get; set; }
    public int OrderId { get; set; }
    public decimal Vat { get; set; }
    public decimal OrderTotal { get; set; }
    
    public decimal OrderSubtotal
    {
        get
        {
            decimal subtotal = 0;
            foreach (OrderItem item in Order.Items)
            {
                subtotal += item.LineTotal;
            }
            return subtotal;
        }
    }
    
    private Order _order;

    public Order Order
    {
        get
        {
            return _order;
        }
        set
        {
            _order = value;
            
            OrderTotal = OrderSubtotal + Vat;
            
            decimal vat = 0;
            foreach (OrderItem item in Order.Items)
            {
                if (item.MenuItem.Card == ItemCard.ALCOHOLIC_DRINKS)
                {
                    vat += item.LineTotal * 0.21m;
                }
                else
                {
                    vat += item.LineTotal * 0.09m;
                }
            }
            Vat = vat;
            
            OrderId = Order.OrderId;
        }
    }

    public Bill()
    {
        BillId = 0;
        OrderId = 0;
        OrderTotal = 0;
        Vat = 0;
    }

    public Bill(int billId, int orderId, decimal orderTotal, int vat)
    {
        BillId = billId;
        OrderId = orderId;
        OrderTotal = orderTotal;
        Vat = vat;
    }
}
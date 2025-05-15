namespace OrderingSystemProject.Models;

public class Bill
{
    public int BillId { get; set; }
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }

    public Bill()
    {
        BillId = 0;
        OrderId = 0;
        TotalAmount = 0;
    }

    public Bill(int billId, int orderId, decimal totalAmount)
    {
        BillId = billId;
        OrderId = orderId;
        TotalAmount = totalAmount;
    }
}
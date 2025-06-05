using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services;

public class PaymentService : IPaymentService
{
    private Bill? _current_bill = null;
    private Payment? _current_payment = null;

    public Bill? GetNewBill(int orderId)
    {
        var order = CommonRepository._order_rep.GetById(orderId);
        if (order == null)
            return null;

        // ✅ Correct usage now
        var bill = CommonRepository._bill_rep.GetByOrderId(orderId);

        if (bill == null)
        {
            bill = new Bill
            {
                Order = order,
                OrderId = order.OrderId,
            };
            CommonRepository._bill_rep.InsertBill(bill);
        }

        _current_bill = bill;
        return bill;
    }

    public Payment? GetNewPayment()
    {
        if (_current_bill == null)
            return null;

        var payment = new Payment
        {
            Bill = _current_bill,
            BillId = _current_bill.BillId
        };

        _current_payment = payment;
        return payment;
    }
    
    public Payment? GetCurrentPayment() { return _current_payment; }
    public Bill? GetCurrentBill() { return _current_bill; }
}
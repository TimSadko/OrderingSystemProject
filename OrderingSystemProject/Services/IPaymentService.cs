using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IPaymentService
{
    Bill? GetNewBill(int orderId);
    Payment? GetNewPayment();
    Payment? GetCurrentPayment();
    Bill? GetCurrentBill();
    List<Payment> SplitEqually(SplitEquallyViewModel splitEquallyViewModel);
    Payment InsertUpdatedPayment(Payment payment);
    Bill GetBillForPaymentById(Payment payment);
    decimal GetPaymentAmount(Payment payment);
    void SetTipAmount(Payment payment);
}
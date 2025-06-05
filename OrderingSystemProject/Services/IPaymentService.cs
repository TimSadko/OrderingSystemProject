using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IPaymentService
{
    Bill? GetNewBill(int orderId);
    Payment? GetNewPayment();
    Payment? GetCurrentPayment();
    Bill? GetCurrentBill();
}
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IPaymentRepository
{
    public Payment? GetById(int id);
    Payment InsertPayment(Payment payment);
    List<Payment> GetPaymentsByBillId(int billId);
}
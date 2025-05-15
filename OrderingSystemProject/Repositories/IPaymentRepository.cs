using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IPaymentRepository
{
    List<Order> GetAll();
    void Pay(int orderId, int amount);
    void Add(Payment payment);
    bool IsPaymentExist(Payment payment);
    void Split (Payment payment);
}
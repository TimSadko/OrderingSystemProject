using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IPaymentRepository
{
    public Payment? GetById(int id);
}
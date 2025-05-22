using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface IBillRepository
{ 
    public Bill? GetById(int id);
}
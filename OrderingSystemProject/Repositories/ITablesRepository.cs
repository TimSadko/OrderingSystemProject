using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface ITablesRepository
{
    List<Table> GetAllTables();
  
    Table? GetTableById(int tableId);
  
    Table GetTableByNumber(int tableId);
  
    List<Table> GetAllTablesWithOrders();  
}
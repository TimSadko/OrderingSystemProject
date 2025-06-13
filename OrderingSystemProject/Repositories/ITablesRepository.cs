using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface ITablesRepository
{
    List<Table> GetAllTables();
  
    Table? GetTableById(int tableId);
    
    void UpdateTableStatus(int tableId, TableStatus newTableStatus);
}
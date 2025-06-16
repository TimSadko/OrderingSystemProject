// Services/ITablesService.cs
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface ITablesService
{
    List<Table> GetAllTables();
    Table GetTableByNumber(int tableId);
    bool ChangeTableStatus(int tableId, TableStatus newStatus);
}
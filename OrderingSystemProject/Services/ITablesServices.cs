using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface ITablesServices
{
    List<Table> GetAllTables();
    Table GetTableByNumber(int tableId);
}
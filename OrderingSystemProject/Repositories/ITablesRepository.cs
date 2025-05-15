using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories;

public interface ITablesRepository
{
    List<Table> GetAllTables();
    Table GetTableByNumber(int number);
}
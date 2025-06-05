using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services;

public class TablesServices : ITablesServices
{
    private readonly ITablesRepository _tablesRepository;
    
    public TablesServices(ITablesRepository tablesRepository)
    {
        _tablesRepository = tablesRepository;
    }

    public List<Table> GetAllTables()
    {
        return _tablesRepository.GetAllTables();
    }
    
    public Table GetTableByNumber(int tableId)
    {
        return _tablesRepository.GetTableById(tableId);
    }

    public List<Table> GetAllTablesWithOrders()
    {
        return _tablesRepository.GetAllTablesWithOrders();
    }
}
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.ViewModels;

namespace OrderingSystemProject.Services;

public class TablesService : ITablesService
{
    private readonly ITablesRepository _tablesRepository;
    private readonly IOrdersRepository _ordersRepository;
    
    public TablesService(ITablesRepository tablesRepository, IOrdersRepository ordersRepository)
    {
        _tablesRepository = tablesRepository;
        _ordersRepository = ordersRepository;
    }

    public List<Table> GetAllTables()
    {
        return _tablesRepository.GetAllTables();
    }
    
    public Table GetTableByNumber(int tableId)
    {
        return _tablesRepository.GetTableById(tableId);
    }

    public bool ChangeTableStatus(int tableId, TableStatus newStatus)
    {
        if (newStatus == TableStatus.Available)
        {
            // check active orders for table
            List<Order> activeOrders = _ordersRepository.GetActiveOrdersByTable(tableId);
        
            if (activeOrders.Count > 0)
            {
                return false;
            }
        }
        _tablesRepository.UpdateTableStatus(tableId, newStatus);
        return true;
    }
}
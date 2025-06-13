namespace OrderingSystemProject.Models;

public enum TableStatus
{
    Available = 0,
    Occupied = 1,
}
public class Table
{
    public int TableId { get; set; }
    public TableStatus Status { get; set; }
    public int TableNumber { get; set; }
    
    public Table()
    {
        // default constructor...
    }
    
    public Table(int tableId, TableStatus status, int tableNumber)
    {
        TableId = tableId;
        TableNumber = tableNumber;
        Status = status;
    }   
}
namespace OrderingSystemProject.Models;

public enum TableStatus
{
    Available = 0,
    Occupied = 1,
    Reserved = 2
}
public class Table
{
    public int TableId { get; set; }
    public int Status { get; set; }
    public Table()
    {
        // default constructor...
    }
    public Table(int tableId, int status)
    {
        TableId = tableId;
        Status = status;
    }
}
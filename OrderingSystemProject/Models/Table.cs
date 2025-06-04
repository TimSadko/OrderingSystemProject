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

    public int Number { get; set; }
    public int Status { get; set; }
    public Table()
    {
        // default constructor...
    }
    public Table(int tableId, int tableNumber, int status)
    {
        TableId = tableId;
        Number = tableNumber;
        Status = status;
    }
}
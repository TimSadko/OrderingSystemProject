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
    public TableStatus Status { get; set; }
    public int TableNumber { get; set; }

    public Table()
    {
        // default constructor...
    }

	public Table(int tableId, TableStatus status, int tableNumber)
	{
		TableId = tableId;
		Status = status;
		TableNumber = tableNumber;
	}
}
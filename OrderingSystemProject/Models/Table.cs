namespace OrderingSystemProject.Models;

public enum TableStatus
{
    Occupied = 1,
    Available = 2
}
public class Table
{
    public int Number { get; set; }
    public TableStatus Status { get; set; }
    public int EmployeeID { get; set; } // maybe useless

    public Table()
    {
        // default constructor...
    }

    public Table(int number, TableStatus status, int employeeID)
    {
        Number = number;
        Status = status;
        EmployeeID = employeeID;
    }
}
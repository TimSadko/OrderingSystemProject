namespace OrderingSystemProject.Models;

public enum EmployeeType
{
    Waiter = 0,
    Bar = 1,
    Kitchen = 2,
    Manager = 3,
    Admin = 4
}

public class Employee
{
    public int EmployeeID { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int EmployeeType { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public Employee()
    {
        // default constructor...
    }
    
    public Employee(int employeeID, string userName, string password, int employeeType, string firstName, string lastName, string email)
    {
        EmployeeID = employeeID;
        UserName = userName;
        Password = password;
        EmployeeType = employeeType;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
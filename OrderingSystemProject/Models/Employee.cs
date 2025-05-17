namespace OrderingSystemProject.Models
{
    public enum EmployeeType
    {
        Waiter = 0, Cook = 1, Bartender = 2, Manager = 3, Admin = 4
    }

    public class Employee
    {
		public int EmployeeId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public EmployeeType EmployeeType { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		public Employee() { }

		public Employee(int employeeId, string userName, string password, EmployeeType employeeType, string firstName, string lastName, string email)
		{
			EmployeeId = employeeId;
			UserName = userName;
			Password = password;
			EmployeeType = employeeType;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
		}
	}
}
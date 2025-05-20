namespace OrderingSystemProject.Models
{
    public enum EmployeeType
    {
        Waiter = 0, Cook = 1, Bartender = 2, Manager = 3, Admin = 4
    }

    public class Employee
    {
        private int _employee_id;
        private string _user_name;
        private string _password;
        private EmployeeType _employee_type;
        private string _first_name;
        private string _last_name;
        private string _email;

        public Employee() { }

        public Employee(int employee_id, string user_name, string password, EmployeeType employee_type, string first_name, string last_name, string email)
        {
            _employee_id = employee_id;
            _user_name = user_name;
            _password = password;
            _employee_type = employee_type;
            _first_name = first_name;
            _last_name = last_name;
            _email = email;
        }

        public int EmployeeId { get => _employee_id; set => _employee_id = value; }
        public string UserName { get => _user_name; set => _user_name = value; }
        public string Password { get => _password; set => _password = value; }
        public EmployeeType EmployeeType { get => _employee_type; set => _employee_type = value; }
        public string FirstName { get => _first_name; set => _first_name = value; }
        public string LastName { get => _last_name; set => _last_name = value; }
        public string Email { get => _email; set => _email = value; }
    }
}
namespace OrderingSystemProject.Models
{
    public enum EMPLOYEE_TYPE
    {
        WAITER = 0, COOK = 1, MANAGER = 2, ADMIN = 3
    }

    public class Employee
    {
        private string _login;
        private EMPLOYEE_TYPE _employee_type;
        private string _email;
        private string _first_name;
        private string _last_name;
        private string _password;

        public Employee() { }

        public Employee(string login, EMPLOYEE_TYPE employee_type, string email, string first_name, string last_name, string password)
        {
            _login = login;
            _employee_type = employee_type;
            _email = email;
            _first_name = first_name;
            _last_name = last_name;
            _password = password;
        }

        public string Login { get => _login; set => _login = value; }
        public EMPLOYEE_TYPE EmployeeType { get => _employee_type; set => _employee_type = value; } 
        public string Email { get => _email; set => _email = value; }
        public string FirstName { get => _first_name; set => _first_name = value; }
        public string LastName { get => _last_name; set => _last_name = value; }
        public string Password { get => _password; set => _password = value; }
    }
}

namespace OrderingSystemProject.Models
{
    public enum EMPLOYEE_TYPE
    {
        WAITER = 0, COOK = 1, MANAGER = 2, ADMIN = 3
    }

    public class Employee
    {
        private int _employee_id;
        private string _login;
		private string _password;
		private EMPLOYEE_TYPE _employee_type;       
        private string _first_name;
        private string _last_name;
		private string _email;

		public Employee() { }

		public Employee(int employee_id, string login, string password, EMPLOYEE_TYPE employee_type, string first_name, string last_name, string email)
		{
			_employee_id = employee_id;
			_login = login;
			_password = password;
			_employee_type = employee_type;
			_first_name = first_name;
			_last_name = last_name;
			_email = email;
		}

		public int EmployeeId { get => _employee_id; set => _employee_id = value; }
        public string Login { get => _login; set => _login = value; }
		public string Password { get => _password; set => _password = value; }
		public EMPLOYEE_TYPE EmployeeType { get => _employee_type; set => _employee_type = value; }        
        public string FirstName { get => _first_name; set => _first_name = value; }
        public string LastName { get => _last_name; set => _last_name = value; }
        public string Email { get => _email; set => _email = value; }
    }
}

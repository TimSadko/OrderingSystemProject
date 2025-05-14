namespace OrderingSystemProject.Models;

public class LoginModel
{
    public string Login { get; set; }
    public string Password { get; set; }

    public LoginModel()
    {
        // default constructor...
    }

    public LoginModel(string login, string password)
    {
        Login = login;
        Password = password;
    }
}
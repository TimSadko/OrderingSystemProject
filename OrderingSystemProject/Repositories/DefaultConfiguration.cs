namespace OrderingSystemProject.Repositories
{
    public class DefaultConfiguration
    {
        private readonly string _connection_string;

        public DefaultConfiguration(string connection_string)
        {
            _connection_string = connection_string;
        }

        public string GetConnectionString()
        {
            return _connection_string;
        }
    }
}

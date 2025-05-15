using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public class MenuItemDB : IMenuItemDB
    {
        private readonly string _connection_string;

        public MenuItemDB(DefaultConfiguration config)
        {
            _connection_string = config.GetConnectionString();
        }

        public List<MenuItem> GetAll()
        {
            List<MenuItem> items = new List<MenuItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query =
                    "SELECT ItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems ORDER BY Name";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem item = ReadItem(reader);
                    items.Add(item);
                }

                reader.Close();
            }

            return items;
        }

        public void Add(MenuItem menuItem)
        {
            using (var connection = new SqlConnection(_connection_string))
            {
                string query = $"INSERT INTO MenuItems (Name, Price, Card, Category, Stock, IsActive) " +
                               $"VALUES (@Name, @Price, @Card, @Category, @Stock, @IsActive) " +
                               $"SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", menuItem.Name);
                command.Parameters.AddWithValue("@Price", menuItem.Price);
                command.Parameters.AddWithValue("@Card", menuItem.Card);
                command.Parameters.AddWithValue("@Category", menuItem.Category);
                command.Parameters.AddWithValue("@Stock", menuItem.Stock);
                command.Parameters.AddWithValue("@IsActive", menuItem.IsActive);
                connection.Open();

                if (command.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Menu item creation failed!");
                }
            }
        }

        private MenuItem ReadItem(SqlDataReader reader)
        {
            return new MenuItem(
                (int)reader["ItemId"],
                (string)reader["Name"],
                (decimal)reader["Price"],
                (ItemCard)(int)reader["Card"],
                (ItemCategory)(int)reader["Category"],
                (int)reader["Stock"],
                (bool)reader["IsActive"]
            );
        }
    }
}
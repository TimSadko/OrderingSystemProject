using Microsoft.Data.SqlClient;
using OrderingSystemProject.Models;

namespace OrderingSystemProject.Repositories
{
    public class DbMenuItemsRepository : IMenuItemsRepository
    {
        private readonly string _connection_string;

        public DbMenuItemsRepository(IConfiguration config)
        {
            _connection_string = config.GetConnectionString("OrderingDatabase");
        }

        public List<MenuItem> GetAll()
        {
            List<MenuItem> items = new List<MenuItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query =
                    "SELECT MenuItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems ORDER BY Name";
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
                string query = "INSERT INTO MenuItems (Name, Price, Card, Category, Stock, IsActive) VALUES (@Name, @Price, @Card, @Category, @Stock, @IsActive); SELECT SCOPE_IDENTITY()";
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

        public void Delete(MenuItem menuItem)
        {
            using (SqlConnection connection = new SqlConnection(_connection_string))
            {
                string query = "DELETE FROM MenuItems WHERE MenuItemId = @ItemId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ItemId", menuItem.MenuItemId);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();
            }
        }

        public MenuItem? GetById(int id)
        {
            MenuItem? item = null;

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT MenuItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems WHERE MenuItemId = @Id";
                SqlCommand com = new SqlCommand(query, conn);

                com.Parameters.AddWithValue("@Id", id);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

				if (reader.HasRows)
                {
                    reader.Read();

					item = ReadItem(reader);
				}

                reader.Close();
            }

            return item;
        }

        public void Update(MenuItem menuItem)
        {
            using (var connection = new SqlConnection(_connection_string))
            {
                string query = "UPDATE MenuItems SET Name = @Name, Price = @Price, Card = @Card, Category = @Category, Stock = @Stock, IsActive = @IsActive WHERE MenuItemId = @MenuItemId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", menuItem.Name);
                command.Parameters.AddWithValue("@Price", menuItem.Price);
                command.Parameters.AddWithValue("@Card", menuItem.Card);
                command.Parameters.AddWithValue("@Category", menuItem.Category);
                command.Parameters.AddWithValue("@Stock", menuItem.Stock);
                command.Parameters.AddWithValue("@IsActive", menuItem.IsActive);
                command.Parameters.AddWithValue("@MenuItemId", menuItem.MenuItemId);
                connection.Open();

                if (command.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Menu item update failed!");
                }
            }
        }

        public List<MenuItem> FilterByCategory(ItemCategory? category)
        {
            List<MenuItem> items = new List<MenuItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query =
                    "SELECT MenuItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems WHERE Category = @Category";
                SqlCommand com = new SqlCommand(query, conn);
                com.Parameters.AddWithValue("@Category", category);

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

        public List<MenuItem> FilterByCard(ItemCard? card)
        {
            List<MenuItem> items = new List<MenuItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query =
                    "SELECT MenuItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems WHERE Card = @Card";
                SqlCommand com = new SqlCommand(query, conn);
                com.Parameters.AddWithValue("@Card", card);

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

        public List<MenuItem> FilterByCategoryAndCard(ItemCategory? category, ItemCard? card)
        {
            List<MenuItem> items = new List<MenuItem>();

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query =
                    "SELECT MenuItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems WHERE Card = @Card AND Category = @Category";
                SqlCommand com = new SqlCommand(query, conn);
                com.Parameters.AddWithValue("@Category", category);
                com.Parameters.AddWithValue("@Card", card);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem item = ReadItem(reader);
                    items.Add(item);
                }

                reader.Close();
            }

            return items;        }

        private MenuItem ReadItem(SqlDataReader reader)
        {
            return new MenuItem(
                (int)reader["MenuItemId"],
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

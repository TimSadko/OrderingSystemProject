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
                string query = "SELECT ItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems ORDER BY Name";
                SqlCommand com = new SqlCommand(query, conn);

                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                MenuItem itm;

                while (reader.Read())
                {
                    itm = ReadMenuItem(reader);
                    items.Add(itm);
                }
                reader.Close();
            }

            return items;
        }

        public MenuItem? GetMenuItem(int menuItemId)
        {
            MenuItem? menuItem = null;

            using (SqlConnection conn = new SqlConnection(_connection_string))
            {
                string query = "SELECT ItemId, Name, Price, Card, Category, Stock, IsActive From MenuItems WHERE ItemId = @ItemId ORDER BY Name";
                SqlCommand com = new SqlCommand(query, conn);

                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.AddWithValue("@ItemId", menuItemId);
                com.Connection.Open();
                SqlDataReader reader = com.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                if (reader.Read())
                {
                    menuItem = ReadMenuItem(reader);
                }
                reader.Close();
            }

            return menuItem;
        }

        private MenuItem ReadMenuItem(SqlDataReader reader)
        {
            return new MenuItem((int)reader["ItemId"], (string)reader["Name"], (decimal)reader["Price"], (ItemCard)(int)reader["Card"], (ItemCategory)reader["Category"], (int)reader["Stock"], (bool)reader["IsActive"]);
        }
    }
}

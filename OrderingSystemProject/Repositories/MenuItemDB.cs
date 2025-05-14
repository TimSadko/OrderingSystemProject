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
                string query = "SELECT ItemId, Name, Price, Card, Category, Stock From MenuItems ORDER BY Name";
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

        private MenuItem ReadItem(SqlDataReader reader)
        {
            return new MenuItem(
                (int)reader["ItemId"],
                (string)reader["Name"],
                (decimal)reader["Price"],
                (ITEM_CARD)(int)reader["Card"],
                (ITEM_CATEGORY)reader["Category"],
                (int)reader["Stock"]
            );
        }
    }
}
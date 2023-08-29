using MySqlConnector;
using NetCore60.Models;
using System;
using System.Linq;

namespace NetCore60.Services
{
    public class DatabaseService
    {
        //private readonly IConfiguration _configuration;
        private readonly string? _connectionString;


        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyDatabaseConnection");
        }

        public string Connect()
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString);

            try
            {
                connection.Open();
                return "Connection opened successfully.";
            }
            catch (Exception ex)
            {
                return $"Error opening connection: {ex.Message}";
            }
            finally
            {
                connection.Close();
            }
        }
        public void testDbContext()
        {
            using (var context = new RnNetCoreDbContext())
            {
                // Create
                //var newItem = new User {  Username = "New Task" };
                //context.Users.Add(newItem);
                //context.SaveChanges();
                //Create

                // Read
                var items = context.Users.ToList();
                foreach (var item in items)
                {
                    var str = item.Id + " " + item.Username;
                    var newItem = new RecordLogTable { DataText = str };
                    context.RecordLogTables.Add(newItem);
                    Console.WriteLine(item.Id + " " + item.Username);
                }
                context.SaveChanges();
                //// Update
                //var itemToUpdate = context.Users.FirstOrDefault(item => item.Title == "New Task");
                //if (itemToUpdate != null)
                //{
                //    itemToUpdate.Title = "Updated Task";
                //    context.SaveChanges();
                //}

                //// Delete
                //var itemToDelete = context.Users.FirstOrDefault(item => item.Title == "Updated Task");
                //if (itemToDelete != null)
                //{
                //    context.Users.Remove(itemToDelete);
                //    context.SaveChanges();
                //}
            }
        }
            public List<TodoItem> GetItems()
        {
            List<TodoItem> items = new List<TodoItem>();

            using MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, Title FROM Users";
            using MySqlCommand command = new MySqlCommand(query, connection);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                TodoItem item = new TodoItem
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Title")
                };

                items.Add(item);
            }

            connection.Close();

            return items;
        }
        public void InsertItem(TodoItem item)
        {
            // 插入数据库操作...
        }

        // 其他方法实现...
    }

}

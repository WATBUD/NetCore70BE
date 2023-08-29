using Microsoft.EntityFrameworkCore;
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

                // Read
                var items = context.Users.ToList();
                foreach (var item in items)
                {
                    var str = item.Id + " " + item.Username;
                    var newItem = new RecordLogTable { DataText = str };
                    try
                    {
                        context.RecordLogTables.Add(newItem);
                        context.SaveChanges();
                        Console.WriteLine(item.Id + " " + item.Username);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
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

        public User? GetUserById(int id)
        {
            // 查询数据库中的用户数据，然后将其映射为 UserDto 对象并返回
            using (var context = new RnNetCoreDbContext())
            {

                var userEntity = context.Users.FirstOrDefault(u => u.Id == id);
                return userEntity;

            }
        }

        public string InsertUserAccount(string _username, string _password)
        {
            int generatedId; // 获取自动生成的 ID
            using (var context = new RnNetCoreDbContext())
            {
                if (string.IsNullOrEmpty(_username))// 字符串为空或 null
                {

                    return "帳號字符串为空或 null";
                }
                //Create
                try
                {
                    var newItem = new User { Username = _username, Password= _password };
                    context.Users.Add(newItem);
                    context.SaveChanges();
                    generatedId = newItem.Id; // 获取自动生成的 ID
                }
                catch (Exception ex)
                {
                    var test = new RnNetCoreDbContext();
                    //test.RecordLogTables.Add(new RecordLogTable { DataText = ex.Message+ ex.InnerException });

                    var sqlExceptionMessage = "" + ex.InnerException?.Message;
                    test.RecordLogTables.Add(new RecordLogTable { DataText = sqlExceptionMessage });

                    test.SaveChanges();
                    Console.WriteLine("Error: " + sqlExceptionMessage); // 打印错误消息
                    return sqlExceptionMessage;
                    throw; // 将异常重新抛出，继续传播异常
                }

                // Read
                var items = context.Users.ToList();
                foreach (var item in items)
                {
                    var str = item.Id + " " + item.Username;
                    try
                    {
                        context.RecordLogTables.Add(new RecordLogTable { DataText = str });
                        context.SaveChanges();
                        Console.WriteLine(item.Id + " " + item.Username);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
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
                return "Account successfully created => userID : " +generatedId.ToString(); //"Account successfully created";
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

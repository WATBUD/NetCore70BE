using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NetCore60.Models;
using System;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;

namespace NetCore60.Services
{
    public class RNDatingService
    {
        //private readonly IConfiguration _configuration;
        private readonly string? _connectionString;


        public RNDatingService(IConfiguration configuration)
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
            using (var context = new RndatingDbContext())
            {
                // Create
                //var newItem = new User {  Username = "New Task" };
                //context.Users.Add(newItem);
                //context.SaveChanges();
                // Read
                var items = context.Users.ToList();
                foreach (var item in items)
                {
                    var str = item.UserId + " " + item.Username;
                    var newItem = new RecordLogTable { DataText = str };
                    try
                    {
                        context.RecordLogTables.Add(newItem);
                        context.SaveChanges();
                        Console.WriteLine(item.UserId + " " + item.Username);
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
            using (var context = new RndatingDbContext())
            {

                var userEntity = context.Users.FirstOrDefault(u => u.UserId == id);
                return userEntity;

            }
        }
        public VUsersDetail? UpdateUserPassword(int user_id,string _password)
        {
            // 查询数据库中的用户数据，然后将其映射为 UserDto 对象并返回
            using (var dbContext = new RndatingDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var userEntity = dbContext.Users.FirstOrDefault(u => u.UserId == user_id);
                        if (userEntity != null)
                        {
                            userEntity.Password = _password;
                            dbContext.SaveChanges();
                        }
                        var returnEntity = dbContext.VUsersDetails.FirstOrDefault(u => u.UserId == user_id);
                        // 提交事务
                        transaction.Commit();
                        return returnEntity;
                    }
                    catch (Exception)
                    {
                        // 如果出现异常，回滚事务
                        transaction.Rollback();
                        return null;
                    }
                }
     
            }
        }

        public object? UpdateUserDetail(VUsersDetail _VUsersDetail)
        {
            if (DataInspectionAndProcessingService.IsValidEmail(_VUsersDetail.Email) == false)
            {
                return "Email格式不正確,請輸入正確格式";
            }
            // 查询数据库中的用户数据，然后将其映射为 UserDto 对象并返回
            using (var dbContext = new RndatingDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var userEntity = dbContext.Users.FirstOrDefault(u => u.UserId == _VUsersDetail.UserId);
                        var userDetailEntity = dbContext.UserDetails.FirstOrDefault(u => u.UdUserId == _VUsersDetail.UserId);

                        if (userEntity != null)
                        {
                            foreach (var propertyInfo in typeof(User).GetProperties())
                            {
                                var newValue = typeof(VUsersDetail).GetProperty(propertyInfo.Name)?.GetValue(_VUsersDetail);
                                if (newValue != null&& propertyInfo.Name!= "UserId")
                                {
                                    propertyInfo.SetValue(userEntity, newValue);
                                }
                            }
                        }
                        if (userDetailEntity != null)
                        {
                            foreach (var propertyInfo in typeof(UserDetail).GetProperties())
                            {
                                var newValue = typeof(VUsersDetail).GetProperty(propertyInfo.Name)?.GetValue(_VUsersDetail);
                                if (newValue != null && propertyInfo.Name != "UdUserId")
                                {
                                    propertyInfo.SetValue(userDetailEntity, newValue);
                                }
                            }
                        }
                        dbContext.SaveChanges();
                        // 提交事务
                        transaction.Commit();
                        var returnEntity = dbContext.VUsersDetails.FirstOrDefault(u => u.UserId == _VUsersDetail.UserId);
                        return returnEntity;
                    }
                    catch (Exception ex)
                    {
                        // 如果出现异常，回滚事务
                        transaction.Rollback();
                        if (ex.InnerException != null)
                        {
                            return ex.InnerException.Message;
                        }
                        else
                        {
                            return ex.Message;
                        }

                    }
                }

            }
        }

        public VUsersDetail? GetUserDetail(int user_id)
        {
            using (var context = new RndatingDbContext())
            {

                var userEntity = context.VUsersDetails.FirstOrDefault(u => u.UdUserId == user_id);
                return userEntity;
                //return new VUsersDetail();
            }
        }
        public List<RequestLog> GetRequestLogs()
        {
            using (var context = new RndatingDbContext())
            {

                var userEntity = context.RequestLogs.ToList();
                return userEntity;
            }
        }
        public List<VTagGroupDetail> GetTagGroupDetails()
        {
            using (var context = new RndatingDbContext())
            {

                var userEntity = context.VTagGroupDetails.ToList();
                return userEntity;
            }
        }

        public string InsertUserAccount(string _account, string password, string _email)
        {
            int generatedId; // 获取自动生成的 ID
            using (var context = new RndatingDbContext())
            {
                if (string.IsNullOrEmpty(_account))// 字符串为空或 null
                {

                    return "帳號为空或 null";
                }
                //Create
                try
                {
                    var newItem = new User { Account = _account, Password = password,Username="新使用者", Email= _email };
                    context.Users.Add(newItem);
                    context.SaveChanges();
                    generatedId = newItem.UserId; // 获取自动生成的 ID
                }
                catch (Exception ex)
                {
                    var test = new RndatingDbContext();
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
                    var str = item.UserId + " " + item.Username;
                    try
                    {
                        context.RecordLogTables.Add(new RecordLogTable { DataText = str });
                        context.SaveChanges();
                        Console.WriteLine(item.UserId + " " + item.Username);
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
                return "Account successfully created => userID : " + generatedId.ToString(); //"Account successfully created";
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

        public string testConnectionDatabase()
        {
            using (var context = new RndatingDbContext())
            {
                try
                {
                    context.Database.OpenConnection();
                    bool isConnected = context.Database.CanConnect();
                    if (isConnected)
                    {
                        return "成功連線到資料庫！";
                    }
                    else
                    {
                        return "無法連線到資料庫。";
                    }
                }
                catch (Exception ex)
                {
                    return "連線時發生錯誤：" + ex.Message;
                }
            }
        }

        public bool CheckDatabaseConnection()
        {
            try
            {
                using var dbContext = new RndatingDbContext();
                dbContext.Database.OpenConnection();
                dbContext.Database.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using NetCore60.DTO;
using NetCore60.Models;
using NetCore60.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace NetCore60.Services
{
    public class RNDatingService
    {
        private readonly ApplicationDbContext _dbContext;

        public RNDatingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Object? checkUserBasicInformation(int id)
        {
                var userEntity = _dbContext.Users.Where(u => u.UserId == id).Select(data => new
                {
                    data.Account,
                    data.Username,
                    data.Email,
                    data.CreatedAt,
                }).FirstOrDefault();
                return userEntity; 
        }

        public User? UpdateUserPassword(int userId, string password)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var userEntity = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
                    if (userEntity != null)
                    {
                        userEntity.Password = password;
                        _dbContext.SaveChanges();
                    }


                    transaction.Commit();
                    return userEntity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }



        public List<RequestLog> GetRequestLogs()
        {
                var userEntity = _dbContext.RequestLogs.ToList();
                return userEntity;
            
        }

        public List<VTagGroupDetail> GetTagGroupDetails()
        {

                var userEntity = _dbContext.VTagGroupDetails.ToList();
                return userEntity;
            
        }

        public string InsertUserAccount(string _account, string password, string _email)
        {
            int generatedId;

                if (string.IsNullOrEmpty(_account))
                {

                    return "帳號为空或 null";
                }
                //Create
                try
                {
                    var newItem = new User { Account = _account, Password = password, Username = "新使用者", Email = _email };
                    _dbContext.Users.Add(newItem);
                    _dbContext.SaveChanges();
                    generatedId = newItem.UserId; 
                }
                catch (Exception ex)
                {
                    var sqlExceptionMessage = "" + ex.InnerException?.Message;
                    //_dbContext.RecordLogTables.Add(new RecordLogTable { DataText = sqlExceptionMessage });
                    //_dbContext.SaveChanges();
                    Console.WriteLine("Error: " + sqlExceptionMessage);
                    return sqlExceptionMessage;
                    //throw; 
                }

                // Read
                var items = _dbContext.Users.ToList();
                foreach (var item in items)
                {
                    var str = item.UserId + " " + item.Username;
                    try
                    {
                        _dbContext.RecordLogTables.Add(new RecordLogTable { DataText = str });
                        _dbContext.SaveChanges();
                        Console.WriteLine(item.UserId + " " + item.Username);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
                return "Account successfully created => userID : " + generatedId.ToString(); //"Account successfully created";
            

        }


        public string testConnectionDatabase()
        {

                try
                {
                    _dbContext.Database.OpenConnection();
                    bool isConnected = _dbContext.Database.CanConnect();
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
        

        public bool CheckDatabaseConnection()
        {
            try
            {
                _dbContext.Database.OpenConnection();
                _dbContext.Database.CloseConnection();
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

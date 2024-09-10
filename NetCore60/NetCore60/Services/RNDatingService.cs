using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using NetCore60.Models;
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
        private readonly RndatingDbContext _dbContext;

        public RNDatingService(RndatingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Object? checkUserBasicInformation(int id)
        {
                var userEntity = _dbContext.VUsersDetails.Where(u => u.UserId == id).Select(data => new
                {
                    data.Account,
                    data.Username,
                    data.Email,
                    data.CreatedAt,
                }).FirstOrDefault();
                return userEntity; 
        }

        public VUsersDetail? UpdateUserPassword(int userId, string password)
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

                    var returnEntity = _dbContext.VUsersDetails.FirstOrDefault(u => u.UserId == userId);

                    transaction.Commit();
                    return returnEntity;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public object? UpdateUserDetail(VUsersDetailDTO _VUsersDetailDTO)
        {
            if (_VUsersDetailDTO.UserId == null)
            {
                return "用戶ID不存在";
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var userEntity = _dbContext.Users.FirstOrDefault(u => u.UserId == _VUsersDetailDTO.UserId);
                var userDetailEntity = _dbContext.UserDetails.FirstOrDefault(u => u.UdUserId == _VUsersDetailDTO.UserId);

                if (userEntity != null)
                {
                    foreach (var propertyInfo in typeof(User).GetProperties())
                    {
                        var newValue = typeof(VUsersDetailDTO).GetProperty(propertyInfo.Name)?.GetValue(_VUsersDetailDTO);
                        if (newValue != null && propertyInfo.Name != "UserId")
                        {
                            propertyInfo.SetValue(userEntity, newValue);
                        }
                    }
                }

                if (userDetailEntity != null)
                {
                    foreach (var propertyInfo in typeof(UserDetail).GetProperties())
                    {
                        var newValue = typeof(VUsersDetailDTO).GetProperty(propertyInfo.Name)?.GetValue(_VUsersDetailDTO);
                        if (newValue != null)
                        {
                            if (propertyInfo.Name == "Birthday")
                            {
                                DateTime dateTime = (DateTime)newValue;
                                if (DataInspectionAndProcessingService.IsAgeAboveThreshold(dateTime, 20))
                                {
                                    return "年齡現代差距過大過大,請輸入正確年齡";
                                }
                                else
                                {
                                    var date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                                    userDetailEntity.Birthday = date;
                                }
                            }
                            else if (propertyInfo.Name == "ProfilePicture")
                            {
                                string newFileName = SystemService.UploadImageFileToServer((IFormFile)newValue, true);
                                userDetailEntity.ProfilePicture = newFileName;
                            }
                            else if (propertyInfo.Name != "UdUserId")
                            {
                                propertyInfo.SetValue(userDetailEntity, newValue);
                            }
                        }
                    }
                }

                _dbContext.SaveChanges();
                transaction.Commit();

                var returnEntity = _dbContext.VUsersDetails.FirstOrDefault(u => u.UserId == _VUsersDetailDTO.UserId);
                return returnEntity;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }


        public int GetLoginUserId(LoginFormModel loginData)
        {
            var userEntity = _dbContext.VUsersDetails.FirstOrDefault(u => u.Account == loginData.Account && u.Password == loginData.Password);

            if (userEntity != null)
            {
                return userEntity.UserId;
            }
            return -1;
        }


        public Object? GetUserDetail(int user_id)
        {
  
                var userEntity = _dbContext.VUsersDetails.Where(u => u.UdUserId == user_id)
                .Select(u => new
                {
                    u.UserId,
                    u.Account,
                    u.Location,
                    u.Interests,
                    // ...
                }).FirstOrDefault();
                return userEntity;
            
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
            int generatedId; // 获取自动生成的 ID

                if (string.IsNullOrEmpty(_account))// 字符串为空或 null
                {

                    return "帳號为空或 null";
                }
                //Create
                try
                {
                    var newItem = new User { Account = _account, Password = password, Username = "新使用者", Email = _email };
                    _dbContext.Users.Add(newItem);
                    _dbContext.SaveChanges();
                    generatedId = newItem.UserId; // 获取自动生成的 ID
                }
                catch (Exception ex)
                {
                    var sqlExceptionMessage = "" + ex.InnerException?.Message;
                    _dbContext.RecordLogTables.Add(new RecordLogTable { DataText = sqlExceptionMessage });
                    _dbContext.SaveChanges();
                    Console.WriteLine("Error: " + sqlExceptionMessage); // 打印错误消息
                    return sqlExceptionMessage;
                    throw; // 将异常重新抛出，继续传播异常
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

using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
namespace NetCore60.Services
{

    public static class DataInspectionAndProcessingService
    {
        public static bool IsValidEmail(string email)
        {
            // 检查电子邮件地址是否为空
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // 电子邮件地址的正则表达式模式
            string pattern = @"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$";

            // 使用正则表达式进行匹配
            Match match = Regex.Match(email, pattern);

            // 如果匹配成功且与整个字符串匹配，则返回true，否则返回false
            return match.Success && match.Value == email;
        }
    }


}

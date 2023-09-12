using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace NetCore60.Services
{

    public static class DataInspectionAndProcessingService
    {
        public static bool IsValidEmail(string ?email)
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
        public static string ToPascalCase(string input)
        {
            // 首先将字符串全部转换为小写
            string lowerCase = input.ToLower();

            // 将字符串分割成单词
            string[] words = lowerCase.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);

            // 转换每个单词的首字母为大写
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(words[i]);
            }

            // 将单词重新组合为字符串
            string pascalCase = string.Join("", words);

            return pascalCase;
        }
    }



}

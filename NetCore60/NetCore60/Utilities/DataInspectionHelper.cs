using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace NetCore60.Utilities
{

    public static class DataInspectionHelper
    {
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            // 电子邮件地址的正则表达式模式
            string pattern = @"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$";
            Match match = Regex.Match(email, pattern);
            return match.Success && match.Value == email;
        }
        public static string ToPascalCase(string input)
        {
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
        public static bool IsAgeAboveThreshold(DateTime? birthdate, int ageThreshold)
        {
            if (birthdate.HasValue)
            {
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - birthdate.Value.Year;

                // 如果生日还未过，年龄减1
                if (currentDate < birthdate.Value.AddYears(age))
                {
                    age--;
                }

                return age >= ageThreshold;
            }
            return false;
        }

    }



}

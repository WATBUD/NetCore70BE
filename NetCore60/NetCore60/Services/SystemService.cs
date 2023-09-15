using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
namespace NetCore60.Services
{

    public static class SystemService
    {
        public static string UploadImageFileToServer(IFormFile file,bool getDrivePath=false)
        {
            if (file != null && file.Length > 0)
            {
                // 验证文件类型
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (allowedExtensions.Contains(fileExtension))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // 使用时间戳生成唯一标识符
                    var newFileName = $"{fileNameWithoutExtension}_{timeStamp}{fileExtension}";
                    var newDrivePath = Path.Combine("F:\\Projectlibrary\\NetCoreBE\\UserImageLibrary", newFileName);

                    using (var stream = new FileStream(newDrivePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return getDrivePath ? newFileName : "updated successfully" ;
                }
                else
                {
                    return "Only supports uploading JPG and PNG files";
                }
            }
            else
            {
                return "No file uploaded";
            }
        }


    }



}
